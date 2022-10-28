using System;
using System.Collections.Generic;
using MyAssets.Scripts.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

//todo:move outside
[Serializable]
public enum PassageType {
    No,
    Door,
    Corridor,
    SecretDoor
};

//todo:move outside
public enum DirectionType {
    North,
    East,
    South,
    West
};

//todo:move outside
public enum BlockType {
    Top,
    InclLeft,
    InclRight,
    NoTop,
    Empty,
    TopBushU,
    TopBushD,
    TopBushUD,
    IncLeftBushU,
    IncLeftBushD,
    IncLeftBushUD,
    IncRightBushU,
    IncRightBushD,
    IncRightBushUD
};

//todo:move outside
[Serializable]
public class EnvironmentStuffingValues {
    public SVector3 location = new SVector3();
    public int indexInPrefabs = 0;

    public EnvironmentStuffingValues(Vector3 v, int i) {
        location = new SVector3(v);
        indexInPrefabs = i;
    }
}

//todo:move outside
[Serializable]
public class EntityValues {
    public int indexInPrefabs = 0;
    public SVector3 location = new SVector3();
    public SVector3 rotation = new SVector3();
    public SVector3 velocity = new SVector3();
    public Inventory inventory = new Inventory();
    public Characteristics characteristics = new Characteristics();

    public EntityValues(int i, Vector3 l, Vector3 r, Vector3 v, Inventory I, Characteristics c) {
        indexInPrefabs = i;
        location = new SVector3(l);
        rotation = new SVector3(r);
        velocity = new SVector3(v);
        inventory = I;
    }
}

//todo:move outside
[Serializable]
public class InhabitationPattern {
    public bool spawnOnce = true;
    public List<int> amounts = new List<int>();
    public List<int> entityPrebabIndexes = new List<int>();
}

[Serializable]
public class MapTile {
    private float growthTimerDelta = 600000f; //10 min
    private float growthTimer = 600000f;
    private EnvironmentFactory environmentFactory;

    public BiomeType biome1 = BiomeType.Forest;
    public BiomeType biome2 = BiomeType.Forest;

    public InhabitationPattern spawnPattern = new InhabitationPattern();

    //todo: replace with grid
    public List<List<int>> blocks = new List<List<int>>();
    public List<EntityValues> TileEntitiesPositions = new List<EntityValues>();
    public List<EnvironmentStuffingValues> TileChests = new List<EnvironmentStuffingValues>();
    public List<Inventory> TileChestsInventry = new List<Inventory>();
    public List<EnvironmentStuffingValues> TileBarricades = new List<EnvironmentStuffingValues>();
    public List<EnvironmentStuffingValues> TilePlatforms = new List<EnvironmentStuffingValues>();

    public List<EnvironmentStuffingValues> TileItems = new List<EnvironmentStuffingValues>();

    //n e s w
    public List<PassageType> passages = new List<PassageType>()
        {PassageType.No, PassageType.No, PassageType.No, PassageType.No};

    public int BlocksInTile = 43;

    public MapTile() {
        environmentFactory = EnvironmentFactory.GetInstance;
    }

    public void GenerateTileStructure() {
        GenerateBlocks();
        GeneratePlatforms();
        AdjustPlatformPositions();
    }

    void GenerateBlocks(int maxDiffs = 0) {
        int temp = 4;
        if (maxDiffs > 4)
            maxDiffs = 4;
        if (maxDiffs < -4)
            maxDiffs = -4;
        for (int i = 0; i < 4; i++) {
            blocks.Add(new List<int>());
            for (int j = 0; j < BlocksInTile; j++)
                blocks[i].Add((int) BlockType.Empty);
        }
        for (int i = 4; i < 9; i++) {
            blocks.Add(new List<int>());
            for (int j = 0; j < BlocksInTile; j++)
                blocks[i].Add((int) BlockType.NoTop);
        }
        FromCenterRow(temp, maxDiffs);
        //FromCenterAndEdges(temp, maxDiffs, MT);
        //FromCenterRowWavy(temp, maxDiffs, MT);
        //PrintTileToFile(MT.blocks, 1,1);
        SetInclinations();
        SetBushesDoors();
    }

    private void SetInclinations() {
        for (int i = 0; i < BlocksInTile; i++) {
            for (int j = 0; j < 9; j++) {
                if (blocks[j][i] == (int) BlockType.NoTop) {
                    if (j != 8) {
                        if ((j == 0) || (blocks[j - 1][i] == (int) BlockType.Empty)) {
                            if ((i == 0) || ((blocks[j + 1][i - 1] != (int) BlockType.Empty) &&
                                             (blocks[j][i - 1] == (int) BlockType.Empty))) {
                                blocks[j][i] = (int) BlockType.IncLeftBushUD;
                            }
                            if ((i == BlocksInTile - 1) || ((blocks[j + 1][i + 1] != (int) BlockType.Empty) &&
                                                            (blocks[j][i + 1] == (int) BlockType.Empty))) {
                                blocks[j][i] = (int) BlockType.IncRightBushUD;
                            }
                            if (((i == 0) || (blocks[j][i - 1] == (int) BlockType.Empty)) &&
                                ((i == BlocksInTile - 1) || (blocks[j][i + 1] == (int) BlockType.Empty))) {
                                blocks[j][i] = (int) BlockType.Empty;
                            }
                            if (((i == 0) || (blocks[j][i - 1] != (int) BlockType.Empty)) &&
                                ((i == BlocksInTile - 1) || (blocks[j][i + 1] != (int) BlockType.Empty)))
                                blocks[j][i] = (int) BlockType.TopBushUD;
                        }
                    }
                }
            }
        }
    }

    private void SetBushesDoors() {
        for (int i = blocks[0].Count / 2 - 4; i < blocks[0].Count / 2 + 3; i++) {
            for (int j = 0; j < 9; j++) {
                if (blocks[j][i] == (int) BlockType.TopBushUD) {
                    if ((j == 0) || (blocks[j - 1][i] == (int) BlockType.Empty)) {
                        if (passages[0] == PassageType.Door) {
                            if (passages[2] == PassageType.Door) {
                                blocks[j][i] = (int) BlockType.Top;
                            } else {
                                blocks[j][i] = (int) BlockType.TopBushD;
                            }
                        } else {
                            if (passages[2] == PassageType.Door) {
                                blocks[j][i] = (int) BlockType.TopBushU;
                            } else {
                                blocks[j][i] = (int) BlockType.TopBushUD;
                            }
                        }
                    }
                }
                if (blocks[j][i] == (int) BlockType.IncLeftBushUD) {
                    if (passages[0] == PassageType.Door) {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = (int) BlockType.InclLeft;
                        } else {
                            blocks[j][i] = (int) BlockType.IncLeftBushD;
                        }
                    } else {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = (int) BlockType.IncLeftBushU;
                        } else {
                            blocks[j][i] = (int) BlockType.IncLeftBushUD;
                        }
                    }
                }
                if (blocks[j][i] == (int) BlockType.IncRightBushUD) {
                    if (passages[0] == PassageType.Door) {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = (int) BlockType.InclRight;
                        } else {
                            blocks[j][i] = (int) BlockType.IncRightBushD;
                        }
                    } else {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = (int) BlockType.IncRightBushU;
                        } else {
                            blocks[j][i] = (int) BlockType.IncRightBushUD;
                        }
                    }
                }
            }
        }
    }

    void FromCenterRow(int middle, int maxDiffs) {
        int temp;
        for (int i = 0; i < BlocksInTile; i++) {
            temp = Random.Range(-maxDiffs, (maxDiffs));
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = j < middle + temp ? (int) BlockType.Empty : (int) BlockType.NoTop;
                //4-empty; 3-solid without topping
            }
        }
    }

    void FromCenterRowWavy(int middle, int maxDiffs) {
        if (maxDiffs + middle > 8)
            maxDiffs = 8 - middle;
        if (maxDiffs > middle)
            maxDiffs = middle;
        int temp = 0;
        int addt = 1;
        for (int i = BlocksInTile / 2; i < BlocksInTile; i++) {
            if ((temp < maxDiffs) && (addt == 1)) {
                temp += addt;
            } else {
                addt = -1;
            }
            if ((temp > -maxDiffs) && (addt == -1)) {
                temp += addt;
            } else {
                addt = 1;
            }
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < middle + temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
        }
        for (int i = BlocksInTile / 2 - 1; i > -1; i--) {
            if ((temp < maxDiffs) && (addt == 1)) {
                temp += addt;
            } else {
                addt = -1;
            }
            if ((temp > -maxDiffs) && (addt == -1)) {
                temp += addt;
            } else {
                addt = 1;
            }
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < middle + temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
        }
    }

    void FromCenterAndEdges(int middle, int maxDiffs) {
        int temp = middle;
        for (int i = 0; i < (BlocksInTile / 4 + 1); i++) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
            /*temp = UnityEngine.Random.Range(-maxDiffs, (maxDiffs+1));
            for (int j = 4 + temp; j != 4- (int)Mathf.Sign(temp); j -= (int)Mathf.Sign(temp))
            {
                MT.blocks[j][i] = ((j < (4-(int)Mathf.Sign(temp))) ? 3 : 4);//4-empty; 3-solid without topping
            }*/
        }
        temp = 4;
        for (int i = BlocksInTile / 2; i > (BlocksInTile / 4); i--) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
        }
        temp = 4;
        for (int i = BlocksInTile / 2 + 1; i < (3 * (BlocksInTile / 4) + 1); i++) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
        }
        temp = 4;
        for (int i = BlocksInTile - 1; i > (3 * (BlocksInTile / 4)); i--) {
            temp += Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++) {
                blocks[j][i] = ((j < temp) ? 4 : 3); //4-empty; 3-solid without topping
            }
        }
    }

    /*void GenerateAllTilesStuffing(List<AllBiomePrefabs> PrefabsOfAllBiomes)
    {
        //fix; both this parameters must be calculated, considering biome and some diapasone of values
        int MaxBGObjectAmount = 10;
        int MaxBarricadesAmount = 10;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Tiles.Tiles [y] [x].FrontalBackgroundIndex = Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].FrontalBackground.Count);
                //Tiles.Tiles [y] [x].RoadIndex = Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Road.Count);
                for (int BGObjectAmount = 0; BGObjectAmount < MaxBGObjectAmount; BGObjectAmount++)
                {
                    Tiles.Tiles [y] [x].TileBackgroundObjects [BGObjectAmount] = PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].BackgroundObjects[Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].BackgroundObjects.Count)];
                    //fix; choose their coordinates somehow
                }
                for (int BarricadesAmount = 0; BarricadesAmount < MaxBarricadesAmount; BarricadesAmount++)
                {
                    Tiles.Tiles [y] [x].TileBarrcicades [BarricadesAmount] = PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Barricades[Random.Range (0, PrefabsOfAllBiomes [(int)Tiles.Tiles [y] [x].biome].Barricades.Count)];
                    //fix; choose their coordinates somehow; maybe add point array, or make new class int+point; maybe make list of objects instead of list of ints
                }
            }
        }
    }*/

    void GeneratePlatforms() {
        for (int i = 0; i < environmentFactory.GetBiomePlatformCount(biome1, biome2); i++) {
            //todo: add platform spawning
            /*if (!BiomePrefabs[(int) Tiles[y][x].biome1][(int) Tiles[y][x].biome2].PlatformPrefab[i]
                .GetComponent<Platform>().sub) {
                Tiles[y][x].TilePlatforms.AddRange(BiomePrefabs[(int) Tiles[y][x].biome1][(int) Tiles[y][x].biome2]
                    .PlatformPrefab[i].GetComponent<Platform>().GeneratePlatforms(Tiles[y][x].blocks));
            }*/
        }
    }

    void AdjustPlatformPositions() {
        int midlevel = 4;
        float inclinedOffsety = 0.0f;
        int j;
        int k;
        for (int i = 0; i < TilePlatforms.Count; i++) //need children of active child of BGPartNumberAsChild
        {
            k = (int) (TilePlatforms[i].location.x / 1.5f) + blocks[0].Count / 2;
            if (k < 0)
                k = 0;
            if (k > blocks[0].Count - 1)
                k = blocks[0].Count - 1;
            for (j = 0; j < 9; j++) {
                if (blocks[j][k] != 4)
                    break;
            }
            TilePlatforms[i].location = new SVector3(TilePlatforms[i].location.x,
                TilePlatforms[i].location.y + (midlevel - j - inclinedOffsety) * 1.5f,
                TilePlatforms[i].location.z + 0.0f);
        }
    } /*
        float inclinedOffsety = 0.0f;
        int j = 0;
        int k = 0;
        for (int i=0; i<Tiles[y][x].TilePlatforms.Count; i++)
        {
            j = 0;
            k = ((int)(Tiles[y][x].TilePlatforms[i].location.x / 1.5f) + Tiles[y][x].blocks[0].Count / 2);
            if (k < 0)
                k = 0;
            if (k > Tiles[y][x].blocks[0].Count - 1)
                k = Tiles[y][x].blocks[0].Count - 1;
            for (j = 0; j < 9; j++)
            {
                if (Tiles[y][x].blocks[j][k] != 4)
                    break;
            }
            if (Tiles[y][x].blocks[j][k] == 1)
            {
                if (k != 0)
                {
                    inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x;
                }
                inclinedOffsety = 1.5f - inclinedOffsety;
            }
            if (Tiles[y][x].blocks[j][k] == 2)
            {
                if (k != 0)
                {
                    inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x % k;
                    if (inclinedOffsety > 1.5f)
                    {
                        inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x % (k - 1);
                    }
                }
                else
                {
                    inclinedOffsety = Tiles[y][x].TilePlatforms[i].location.x;
                }
            }
                Tiles[y][x].TilePlatforms[i].location = new SVector3(Tiles[y][x].TilePlatforms[i].location.x,
                                                                 Tiles[y][x].TilePlatforms[i].location.y + (4-j- inclinedOffsety) *1.5f,
                                                                 Tiles[y][x].TilePlatforms[i].location.z + 0.0f);
        }
    }*/

    /*bool LoadTiles()
	{
		// 1
		//Debug.Log(Application.persistentDataPath);
		if (File.Exists(Application.persistentDataPath + "/gamesave.save"))//"C:/Users/Василий.Василий-ПК/Documents/Grimoire Forest/Exe"
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);//"C:/Users/Василий.Василий-ПК/Documents/Grimoire Forest/Exe"
			try
			{
			Tiles = (SaveLoad)bf.Deserialize(file);
			}
			catch(System.Exception exc)
			{
				Debug.Log ("Whoopsy-doodle!");
				file.Close();
				return false;
			}
			file.Close();
			return true;
		}
		return false;
	}
	public void SaveGame()
	{
		// 1
		/*for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
			{
				if (Save.SavedTiles.Count < (x * Height + y))
					Save.SavedTiles.Add (new TileSave());
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].FrontalBackgroundIndex = Tiles [y] [x].FrontalBackgroundIndex;
				Save.SavedTiles [x * Height + y].RoadIndex = Tiles [y] [x].RoadIndex;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
				Save.SavedTiles [x * Height + y].biome = Tiles [y] [x].biome;
			}
		foreach (GameObject targetGameObject in targets)
		{
			Target target = targetGameObject.GetComponent<Target>();
			if (target.activeRobot != null)
			{
				Save.livingTargetPositions.Add(target.position);
				Save.livingTargetsTypes.Add((int)target.activeRobot.GetComponent<Robot>().type);
				i++;
			}
		}



		Save.hits = hits;
		Save.shots = shots;*/

    // 2
    /*BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");//"C:/Users/Василий.Василий-ПК/Documents/Grimoire Forest/Exe"
    bf.Serialize(file, Tiles);
    file.Close();
}
*/

    public (bool, bool) TryGrow(float deltaTime) {
        bool grow1 = false, grow2 = false;
        if (growthTimer > 0) {
            growthTimer -= deltaTime;
        } else {
            growthTimer = growthTimerDelta;
            int biome1Growth = environmentFactory.GetBiomeGrowth(biome1);
            int biome2Growth = environmentFactory.GetBiomeGrowth(biome2);
            grow1 = Random.Range(0, 100) < biome1Growth;
            grow2 = Random.Range(0, 100) < biome2Growth;
        }
        return (grow1, grow2);
    }

    public void TryChangeToBiome(BiomeType newBiomeType) {
        int newBiomeAggro = environmentFactory.GetBiomeGrowth(newBiomeType);
        int biome1Resist = environmentFactory.GetBiomeResist(biome1) - newBiomeAggro;
        int biome2Resist = environmentFactory.GetBiomeResist(biome2) - newBiomeAggro;
        if (biome1Resist > biome2Resist) {
            if (biome2Resist < 0)
                biome2 = newBiomeType;
        } else {
            if (biome1Resist < 0)
                biome1 = newBiomeType;
        }
    }
}
