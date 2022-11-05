using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using MyAssets.Scripts.Environment;
using Random = UnityEngine.Random;

[Serializable]
public class Map : MonoBehaviour {
    private (bool, bool) tileGrowthResult;

    private int width = 100;
    private int height = 100;
    private int MaximumCorridorLength = 10;
    private int MinimumCorridorLength = 0;

    private EnvironmentFactory environmentFactory;

    [SerializeField]
    private int gridWidth, gridHeight;

    //[NonSerialized]
    //public List<GameObject> GlobalEntities = new List<GameObject>();
    public List<int> GlobalEntitiesLocalIndexes = new List<int>();
    public List<SVector3> GlobalEntitiesTileLocations = new List<SVector3>();
    public List<Biome> Biomes = new List<Biome>();
    public List<List<MapTile>> Tiles = new List<List<MapTile>>();

    public void Init(int W, int H, int MxCL, int MnCL) {
        width = W;
        height = H;
        MaximumCorridorLength = MxCL;
        MinimumCorridorLength = MnCL;
    }

    private void Start() {
        environmentFactory = EnvironmentFactory.GetInstance;
    }

    private void Update() {
        for (int y = 0; y < Tiles.Count; y++) {
            for (int x = 0; x < Tiles[y].Count; x++) {
                tileGrowthResult = Tiles[y][x].TryGrow(Time.deltaTime);
                if (tileGrowthResult.Item1)
                    TryChangeSurroundingTiles(y, x, Tiles[y][x].biome1);
                if (tileGrowthResult.Item2)
                    TryChangeSurroundingTiles(y, x, Tiles[y][x].biome2);
            }
        }
    }

    private void TryChangeSurroundingTiles(int y, int x, BiomeType growingType) {
        if (y - 1 >= 0) Tiles[y - 1][x].TryChangeToBiome(growingType);
        if (y + 1 < Tiles.Count) Tiles[y + 1][x].TryChangeToBiome(growingType);
        if (x + 1 < Tiles[y].Count) Tiles[y][x + 1].TryChangeToBiome(growingType);
        if (x - 1 >= 0) Tiles[y][x - 1].TryChangeToBiome(growingType);
    }

    //it might be better to make this bool and make some checks in case, that generation fails, so we could restart it
    public void Generate_Map() {
        //generate tile's parts and put them in save, then create savegame file, after generation in finished
        //biome creation might be changed further on, for River, for example
        //int min_radius = 5; //min_radius should be initialized from options or in biomes, decide
        for (int i = 1; i < environmentFactory.BiomeAmount; i++) {
            Biomes.Add(new Biome((BiomeType) i, SetBiomeCenter(),
                environmentFactory.GetBiomeRadius((BiomeType) i, width)));
        }
        for (int y = 0; y < height; y++) {
            Tiles.Add(new List<MapTile>());
            for (int x = 0; x < width; x++) {
                Tiles[y].Add(new MapTile());
            }
        }
        SetTiles();
        PrintMapToFile();
        PrintMapToFile(true);
    }

    void SetTiles() {
        GenerateHorizontalCorridors();
        GenerateVerticalCorridors();
        FixCorridors();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                SetTileBiome(x, y);
                Tiles[y][x].Init(gridWidth, gridHeight);
                //todo: move these three in Tile
                //PrintTileToFile(Tiles[y][x].blocks, x, y);
            }
        }
        //SaveGame();
    }

    void SetTileBiome(int x, int y) {
        for (int i = 1; i < environmentFactory.BiomeAmount; i++) {
            if (Vector3.Distance(new Vector3(x, y, 0), Biomes[i].Center) <= Biomes[i].Radius) {
                Tiles[y][x].TryChangeToBiome((BiomeType) i);
            }
        }
    }

    private Vector3 SetBiomeCenter() {
        int x = 0, y = 0;
        bool centerNotChosen = true;
        while (centerNotChosen) {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
            centerNotChosen = false;
            foreach (var biome in Biomes) {
                if (biome.X == x && biome.Y == y)
                    centerNotChosen = true;
            }
        }
        return new Vector3(x, y, 0);
    }

    void GenerateHorizontalCorridors() {
        int corridor_length = 0;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (corridor_length == 0) {
                    if (UnityEngine.Random.Range(0, 2) == 1) {
                        corridor_length = UnityEngine.Random.Range(MinimumCorridorLength, MaximumCorridorLength) + 1;
                        if (UnityEngine.Random.Range(0, 2) == 1) {
                            if (x > 0) {
                                Tiles[y][x - 1].passages[1] = PassageType.Door;
                                Tiles[y][x].passages[3] = PassageType.Door;
                            }
                        }
                    }
                }
                if (corridor_length != 0) {
                    if ((x + 1) < width) {
                        Tiles[y][x].passages[1] = PassageType.Corridor;
                        Tiles[y][x + 1].passages[3] = PassageType.Corridor;
                    }
                    corridor_length--;
                }
            }
        }
    }

    void GenerateVerticalCorridors() {
        int corridor_length = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (corridor_length == 0) {
                    if (UnityEngine.Random.Range(0, 2) == 1) {
                        corridor_length = UnityEngine.Random.Range(MinimumCorridorLength, MaximumCorridorLength) + 1;
                        if (UnityEngine.Random.Range(0, 2) == 1) {
                            if (y > 0) {
                                Tiles[y - 1][x].passages[2] = PassageType.Door;
                                Tiles[y][x].passages[0] = PassageType.Door;
                            }
                        }
                    }
                }
                if (corridor_length != 0) {
                    if ((y + 1) < height) {
                        if ((Tiles[y][x].passages[1] == PassageType.Corridor) ||
                            (Tiles[y][x].passages[3] == PassageType.Corridor)) {
                            if (UnityEngine.Random.Range(0, 2) == 1) {
                                Tiles[y][x].passages[2] = PassageType.Door;
                                Tiles[y + 1][x].passages[0] = PassageType.Door;
                            } else {
                                if (Tiles[y][x].passages[1] == PassageType.Corridor) {
                                    Tiles[y][x].passages[1] = PassageType.Door;
                                    Tiles[y][x + 1].passages[3] = PassageType.Door;
                                }
                                if (Tiles[y][x].passages[3] == PassageType.Corridor) {
                                    Tiles[y][x - 1].passages[1] = PassageType.Door;
                                    Tiles[y][x].passages[3] = PassageType.Door;
                                }
                                if (Tiles[y + 1][x].passages[1] == PassageType.Corridor) {
                                    Tiles[y + 1][x].passages[1] = PassageType.Door;
                                    Tiles[y + 1][x + 1].passages[3] = PassageType.Door;
                                }
                                if (Tiles[y + 1][x].passages[3] == PassageType.Corridor) {
                                    Tiles[y + 1][x - 1].passages[1] = PassageType.Door;
                                    Tiles[y + 1][x].passages[3] = PassageType.Door;
                                }
                                Tiles[y][x].passages[2] = PassageType.Corridor;
                                Tiles[y + 1][x].passages[0] = PassageType.Corridor;
                            }
                        } else {
                            Tiles[y][x].passages[2] = PassageType.Door;
                            Tiles[y + 1][x].passages[0] = PassageType.Door;
                        }
                    }
                    corridor_length--;
                }
            }
        }
    }

    void FixCorridors() {
        for (int y = 0; y < height - 1; y++) {
            for (int x = 0; x < width - 1; x++) {
                if (Tiles[y][x].passages[2] != Tiles[y + 1][x].passages[0]) {
                    Tiles[y][x].passages[2] = FixConnection(Tiles[y][x].passages[2], Tiles[y + 1][x].passages[0]);
                    Tiles[y + 1][x].passages[0] = FixConnection(Tiles[y][x].passages[2], Tiles[y + 1][x].passages[0]);
                }
                if (Tiles[y][x].passages[1] != Tiles[y][x + 1].passages[3]) {
                    Tiles[y][x].passages[1] = FixConnection(Tiles[y][x].passages[1], Tiles[y][x + 1].passages[3]);
                    Tiles[y][x + 1].passages[3] = FixConnection(Tiles[y][x].passages[1], Tiles[y][x + 1].passages[3]);
                }
            }
        }
        for (int x = 0; x < width; x++) {
            Tiles[0][x].passages[0] = PassageType.No;
        }
        for (int y = 0; y < height; y++) {
            Tiles[y][width - 1].passages[1] = PassageType.No;
        }
        for (int x = 0; x < width; x++) {
            Tiles[height - 1][x].passages[2] = PassageType.No;
        }
        for (int y = 0; y < height; y++) {
            Tiles[y][0].passages[3] = PassageType.No;
        }
    }

    PassageType FixConnection(PassageType pass1, PassageType pass2) {
        if ((pass1 == PassageType.No) || (pass2 == PassageType.No)) {
            return PassageType.No;
        }
        if ((pass1 == PassageType.SecretDoor) || (pass2 == PassageType.SecretDoor)) {
            return PassageType.SecretDoor;
        }
        if ((pass1 == PassageType.Door) || (pass2 == PassageType.Door)) {
            return PassageType.Door;
        }
        return PassageType.Corridor;
    }

    void PrintTileToFile(List<List<int>> Tiles, int x1, int y1, bool noPass = false) {
        string name;
        if (noPass) {
            name = "TestTile" + y1.ToString() + x1.ToString() + "1.txt";
        } else {
            name = "TestTile" + y1.ToString() + x1.ToString() + ".txt";
        }
        var sr = File.CreateText(name);
        for (int y = 0; y < 9; y++) {
            for (int x = 0; x < 50; x++) {
                switch (Tiles[y][x]) {
                    case 0:
                        sr.Write("="); //
                        break;
                    case 1:
                        sr.Write("/"); //
                        break;
                    case 2:
                        sr.Write("\\"); //
                        break;
                    case 3:
                        sr.Write("#"); //
                        break;
                    default:
                        break;
                }
            }
            sr.WriteLine();
        }
        sr.Close();
    }

    void PrintMapToFile(bool noPass = false) {
        string name;
        if (noPass) {
            name = "TestMap1.txt";
        } else {
            name = "TestMap.txt";
        }
        var sr = File.CreateText(name);
        for (int y = 0; y < height; y++) {
            if (!noPass) {
                for (int x = 0; x < width; x++) {
                    switch (Tiles[y][x].passages[0]) {
                        case PassageType.No:
                            sr.Write("  "); //
                            break;
                        case PassageType.Door:
                            sr.Write(" |"); //
                            break;
                        case PassageType.SecretDoor:
                            sr.Write(" :"); //
                            break;
                        case PassageType.Corridor:
                            sr.Write(" H"); //
                            break;
                        default:
                            break;
                    }
                }
                sr.WriteLine();
            }
            for (int x = 0; x < width; x++) {
                if (!noPass) {
                    switch (Tiles[y][x].passages[3]) {
                        case PassageType.No:
                            sr.Write(" ");
                            break;
                        case PassageType.Door:
                            sr.Write("-");
                            break;
                        case PassageType.SecretDoor:
                            sr.Write("~");
                            break;
                        case PassageType.Corridor:
                            sr.Write("=");
                            break;
                        default:
                            break;
                    }
                }
                switch (Tiles[y][x].biome1) {
                    case BiomeType.Forest:
                        sr.Write('T');
                        break;
                    case BiomeType.Dump:
                        sr.Write('%');
                        break;
                    /*case BiomesTypes.DumpWaterfall:
                        sr.Write ('!');
                        break;
                    case BiomesTypes.DumpShroom:
                        sr.Write ('&');
                        break;
                    case BiomesTypes.DumpRiver:
                        sr.Write ('@');
                        break;
                    case BiomesTypes.DumpCaves:
                        sr.Write ('8');
                        break;
                    case BiomesTypes.DumpSwamp:
                        sr.Write ('+');
                        break;
                    case BiomesTypes.DumpMill:
                        sr.Write ('M');
                        break;
                    case BiomesTypes.DumpMaze:
                        sr.Write ('=');
                        break;*/
                    case BiomeType.Waterfall:
                        sr.Write('|');
                        break;
                    /*case BiomesTypes.WaterfallShroom:
                        sr.Write ('F');
                        break;
                    case BiomesTypes.WaterfallRiver:
                        sr.Write ('^');
                        break;
                    case BiomesTypes.WaterfallCaves:
                        sr.Write ('0');
                        break;
                    case BiomesTypes.WaterfallSwamp:
                        sr.Write ('K');
                        break;
                    case BiomesTypes.WaterfallMill:
                        sr.Write ('[');
                        break;
                    case BiomesTypes.WaterfallMaze:
                        sr.Write ('{');
                        break;*/
                    case BiomeType.Shroom:
                        sr.Write('P');
                        break;
                    /*case BiomesTypes.ShroomRiver:
                        sr.Write ('R');
                        break;
                    case BiomesTypes.ShroomCaves:
                        sr.Write ('9');
                        break;
                    case BiomesTypes.ShroomSwamp:
                        sr.Write ('*');
                        break;
                    case BiomesTypes.ShroomMill:
                        sr.Write ('p');
                        break;
                    case BiomesTypes.ShroomMaze:
                        sr.Write ('Y');
                        break;*/
                    case BiomeType.River:
                        sr.Write('~');
                        break;
                    /*case BiomesTypes.RiverCaves:
                        sr.Write ('s');
                        break;
                    case BiomesTypes.RiverSwamp:
                        sr.Write ('w');
                        break;
                    case BiomesTypes.RiverMill:
                        sr.Write ('a');
                        break;
                    case BiomesTypes.RiverMaze:
                        sr.Write ('$');
                        break;*/
                    case BiomeType.Caves:
                        sr.Write('O');
                        break;
                    /*case BiomesTypes.CavesSwamp:
                        sr.Write ('C');
                        break;
                    case BiomesTypes.CavesMill:
                        sr.Write ('B');
                        break;
                    case BiomesTypes.CavesMaze:
                        sr.Write ('G');
                        break;*/
                    case BiomeType.Swamp:
                        sr.Write('x');
                        break;
                    /*case BiomesTypes.SwampMill:
                        sr.Write ('y');
                        break;
                    case BiomesTypes.SwampMaze:
                        sr.Write ('W');
                        break;*/
                    case BiomeType.Mill:
                        sr.Write('A');
                        break;
                    /*case BiomesTypes.MillMaze:
                        sr.Write ('H');
                        break;*/
                    case BiomeType.Maze:
                        sr.Write('#');
                        break;
                    default:
                        sr.Write('.');
                        break;
                }
                /*switch (Tiles [y] [x].eastern_passage)
                {
                    case PassageType.No:
                        sr.Write (" ");
                        break;
                    case PassageType.Door:
                        sr.Write ("-");
                        break;
                    case PassageType.SecretDoor:
                        sr.Write ("~");
                        break;
                    case PassageType.Corridor:
                        sr.Write ("=");
                        break;
                    default:
                        break;
                }*/
            }
            sr.WriteLine();
            /*for (int x = 0; x < Width; x++)
            {
                switch (Tiles [y] [x].southern_passage)
                {
                    case PassageType.No:
                        sr.Write ("   ");
                        break;
                    case PassageType.Door:
                        sr.Write (" | ");
                        break;
                    case PassageType.SecretDoor:
                        sr.Write (" : ");
                        break;
                    case PassageType.Corridor:
                        sr.Write (" H ");
                        break;
                    default:
                        break;
                }
            }
            sr.WriteLine ();*/
        }
        sr.Close();
    }

    public Vector3 MapCenter => Biomes.Count > 0 ? Biomes[0].Center : new Vector3(width / 2, height / 2, 0);
    public int GridWidth => gridWidth;
    public int Width => width;
    public int Height => height;
}
