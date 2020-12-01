using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Unity.Mathematics;
[Serializable]
public class Map
{
	
	public int Width;
	public int Height;
	int MaximumCorridorLength;
	int MinimumCorridorLength;
    public int BlocksInTile = 43;
    //[NonSerialized]
    //public List<GameObject> GlobalEntities = new List<GameObject>();
    public List<int> GlobalEntitiesLocalIndexes = new List<int>();
    public List<SVector3> GlobalEntitiesTileLocations = new List<SVector3>(); 
	public List<Biome> Biomes = new List<Biome>();
    public List<List<MapTile>> Tiles = new List<List<MapTile>>();

    public Map()
    {
        Width = 100;
        Height = 100;
        MaximumCorridorLength = 10;
        MinimumCorridorLength = 0;
    }
    public Map(int W, int H, int MxCL, int MnCL)
    {
        Width = W;
        Height = H;
        MaximumCorridorLength = MxCL;
        MinimumCorridorLength = MnCL;
    }

	public void Generate_Map(List<BiomeTilesData> BTD)//it might be better to make this bool and make some checks in case, that generation fails, so we could restart it
	{
		//generate tile's parts and put them in save, then create savegame file, after generation in finished
		//biome creation might be changed further on, for River, for example
		int min_radius = 5;//min_radius should be initialized from options
        for (int i = 0; i < BiomeTilesData.BiomesAmount; i++)
        {
            Biomes.Add(new Biome((BiomesTypes)i));
			Biomes[i].radius = SetBiomeRadius(min_radius, BTD, i);
			SetBiomeCenter(i);
        }
		for (int y = 0; y < Height; y++)
		{
			Tiles.Add (new List<MapTile>());
			for (int x = 0; x < Width; x++)
			{
				Tiles[y].Add (new MapTile ());
            }
		}
		//if(!LoadTiles ())
		    SetTiles();
        PrintMapToFile();
        PrintMapToFile (true);
    }
    void GenerateBlocks(MapTile MT, int x, int y, int maxDiffs = 0)
    {
        int temp=4;
        if (maxDiffs > 4)
            maxDiffs = 4;
        if (maxDiffs < -4)
            maxDiffs = -4;
        for (int i = 0; i < 4; i++)
        {
            MT.blocks.Add(new List<int>());
            for (int j = 0; j < BlocksInTile; j++)
                MT.blocks[i].Add(4);
        }
        for (int i = 4; i < 9; i++)
        {
            MT.blocks.Add(new List<int>());
            for (int j = 0; j < BlocksInTile; j++)
                MT.blocks[i].Add(3);//BiomePrefabs[(int)MT.biome1][(int)MT.biome2].BlockPrefabs.Count
        }
        FromCenterRow(temp, maxDiffs, MT);
        //FromCenterAndEdges(temp, maxDiffs, MT);
        for (int i = 0; i < BlocksInTile; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (MT.blocks[j][i] == 3)
                {
                    if (j != 8)
                    {
                        if ((j == 0) || (MT.blocks[j - 1][i] == 4))
                        {
                            if ((i==0)||((MT.blocks[j + 1][i - 1] != 4) && (MT.blocks[j][i - 1] == 4)))
                                MT.blocks[j][i] = 1;
                            if ((i == BlocksInTile - 1)||((MT.blocks[j + 1][i + 1] != 4) && (MT.blocks[j][i + 1] == 4)))
                                MT.blocks[j][i] = 2;
                            if (((i == 0)||(MT.blocks[j][i - 1] == 4)) && ((i == BlocksInTile - 1)||(MT.blocks[j][i + 1] == 4)))
                                MT.blocks[j][i] = 4;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < 60; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (MT.blocks[j][i] == 3)
                {
                    if ((j == 0) || (MT.blocks[j - 1][i] == 4))
                    {
                        MT.blocks[j][i] = 0;
                    }
                }
            }
        }
    }

    void FromCenterRow(int middle, int maxDiffs, MapTile MT)
    {
        int temp;
        for (int i = 0; i < BlocksInTile; i++)
        {
            temp = UnityEngine.Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = middle+temp; j != middle-(int)Math.Sign(temp); j-= Math.Sign(temp))
            {
                MT.blocks[j][i] = ((j <= middle) ? 4 : 3);//4-empty; 3-solid without topping
            }
        }
    }
    void FromCenterAndEdges(int middle, int maxDiffs, MapTile MT)
    {
        int temp = middle;
        for (int i = 0; i < (BlocksInTile / 4 + 1); i++)
        {
            temp += UnityEngine.Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++)
            {
                MT.blocks[j][i] = ((j < temp) ? 4 : 3);//4-empty; 3-solid without topping
            }
            /*temp = UnityEngine.Random.Range(-maxDiffs, (maxDiffs+1));
            for (int j = 4 + temp; j != 4- (int)Mathf.Sign(temp); j -= (int)Mathf.Sign(temp))
            {
                MT.blocks[j][i] = ((j < (4-(int)Mathf.Sign(temp))) ? 3 : 4);//4-empty; 3-solid without topping
            }*/
        }
        temp = 4;
        for (int i = BlocksInTile / 2; i > (BlocksInTile / 4); i--)
        {
            temp += UnityEngine.Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++)
            {
                MT.blocks[j][i] = ((j < temp) ? 4 : 3);//4-empty; 3-solid without topping
            }
        }
        temp = 4;
        for (int i = BlocksInTile / 2 + 1; i < (3 * (BlocksInTile / 4) + 1); i++)
        {
            temp += UnityEngine.Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++)
            {
                MT.blocks[j][i] = ((j < temp) ? 4 : 3);//4-empty; 3-solid without topping
            }
        }
        temp = 4;
        for (int i = BlocksInTile - 1; i > (3 * (BlocksInTile / 4)); i--)
        {
            temp += UnityEngine.Random.Range(-maxDiffs, (maxDiffs + 1));
            for (int j = 0; j < 9; j++)
            {
                MT.blocks[j][i] = ((j < temp) ? 4 : 3);//4-empty; 3-solid without topping
            }
        }/**/
    }
    void SetBiomeCenter(int biome_index)
	{
		//prevents biome centers overlap
		if(biome_index != 0)
		{
			while(true)
			{
				Biomes[biome_index].Center.x = UnityEngine.Random.Range(0, Width);
				Biomes[biome_index].Center.y = UnityEngine.Random.Range(0, Height);
				for(int prev_biomes = 0; prev_biomes < biome_index; prev_biomes++)
				{
					if((Biomes[biome_index].Center.x == Biomes[prev_biomes].Center.x) && (Biomes[biome_index].Center.y == Biomes[prev_biomes].Center.y))
						continue;
				}
				break;
			}
		}
		else
		{
			Biomes[biome_index].radius = 0;
			Biomes[biome_index].Center.x = Width/2;
			Biomes[biome_index].Center.y = Height/2;
		}
	}

    int SetBiomeRadius(int min_radius, List<BiomeTilesData> BTD, int biomeIndex)
    {
        if (biomeIndex > 0)
        {
            return UnityEngine.Random.Range(min_radius, (Width*(int)BTD[biomeIndex].Size)/100);
        }
        else
        {
            return (int)(Math.Sqrt(Width * Width + Height * Height) + 1);
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


	void SetTiles()
	{
        List<List<BiomeTilesData>> B = GameObject.Find("WorldManager").GetComponent<WorldManagement>().BiomePrefabs;
        for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				SetTileBiome (x, y);
                GenerateBlocks(Tiles[y][x], x, y, 2);
                GeneratePlatforms(x, y, B);
                AdjustPlatformPositions(x, y);
                //PrintTileToFile(Tiles[y][x].blocks, x, y);
			}
		}
		GenerateHorizontalCorridors ();
		GenerateVerticalCorridors ();
        FixCorridors();
		//SaveGame();
	}

	void SetTileBiome(int x, int y)
	{
        int chosen = 0;
        float shortestDistance = 100000.0f;
        float newDistance;
        for (int i = 1; i < BiomeTilesData.BiomesAmount; i++)
        {
            newDistance = GetDistance(x, y, (int) Biomes[i].Center.x, (int) Biomes[i].Center.y);
            if ((newDistance < shortestDistance)&&(newDistance <= Biomes[i].radius))
            {
                shortestDistance = newDistance;
                chosen = i;
            }
        }
        Tiles[y][x].biome1 = (BiomesTypes)chosen;
	}

    float GetDistance(int x1, int y1, int x2, int y2)
    {
        float x = x1 - x2;
        float y = y1 - y2;
        return (float)(Math.Sqrt(x * x + y * y));
    }

	void GenerateHorizontalCorridors ()
	{
		int corridor_length = 0;
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				if (corridor_length == 0)
				{
					if (UnityEngine.Random.Range (0, 2) == 1)
					{
						corridor_length = UnityEngine.Random.Range (MinimumCorridorLength, MaximumCorridorLength) + 1;
						if (UnityEngine.Random.Range (0, 2) == 1)
						{
							if (x > 0)
							{
								Tiles [y] [x - 1].passages[1] = PassageType.Door;
								Tiles [y] [x].passages[3] = PassageType.Door;
							}
						}
					}
				}
				if (corridor_length != 0)
				{
					if ((x + 1) < Width)
					{
						Tiles [y] [x].passages[1] = PassageType.Corridor;
						Tiles [y] [x+1].passages[3] = PassageType.Corridor;
					}
					corridor_length--;
				}
			}
		}
	}
	void GenerateVerticalCorridors ()
	{
		int corridor_length = 0;
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if (corridor_length == 0)
				{
					if (UnityEngine.Random.Range (0, 2) == 1)
					{
						corridor_length = UnityEngine.Random.Range (MinimumCorridorLength, MaximumCorridorLength) + 1;
						if (UnityEngine.Random.Range (0, 2) == 1)
						{
							if (y > 0)
							{
								Tiles [y - 1] [x].passages[2] = PassageType.Door;
								Tiles [y] [x].passages[0] = PassageType.Door;
							}
						}
					}
				}
				if (corridor_length != 0)
				{
					if ((y + 1) < Height)
					{
						if ((Tiles [y] [x].passages[1] == PassageType.Corridor) || (Tiles [y] [x].passages[3] == PassageType.Corridor))
						{
							if (UnityEngine.Random.Range (0, 2) == 1)
							{
								Tiles [y] [x].passages[2] = PassageType.Door;
								Tiles [y + 1] [x].passages[0] = PassageType.Door;
							}
							else
							{
								if (Tiles [y] [x].passages[1] == PassageType.Corridor)
								{
									Tiles [y] [x].passages[1] = PassageType.Door;
									Tiles [y] [x + 1].passages[3] = PassageType.Door;
								}
								if (Tiles [y] [x].passages[3] == PassageType.Corridor)
								{
									Tiles [y] [x - 1].passages[1] = PassageType.Door;
									Tiles [y] [x].passages[3] = PassageType.Door;
								}
								if (Tiles [y + 1] [x].passages[1] == PassageType.Corridor)
								{
									Tiles [y + 1] [x].passages[1] = PassageType.Door;
									Tiles [y + 1] [x + 1].passages[3] = PassageType.Door;
								}
								if (Tiles [y + 1] [x].passages[3] == PassageType.Corridor)
								{
									Tiles [y + 1] [x - 1].passages[1] = PassageType.Door;
									Tiles [y + 1] [x].passages[3] = PassageType.Door;
								}
								Tiles [y] [x].passages[2] = PassageType.Corridor;
								Tiles [y + 1] [x].passages[0] = PassageType.Corridor;
							}
						}
						else
						{
							Tiles [y] [x].passages[2] = PassageType.Door;
							Tiles [y + 1] [x].passages[0] = PassageType.Door;
						}
					}
					corridor_length--;
				}
			}
		}
	}

    void FixCorridors()
    {
        for (int y = 0; y < Height-1; y++)
        {
            for (int x = 0; x < Width-1; x++)
            {
                if (Tiles[y][x].passages[2] != Tiles[y + 1][x].passages[0])
                {
                    Tiles[y][x].passages[2] = FixConnection(Tiles[y][x].passages[2], Tiles[y + 1][x].passages[0]);
                    Tiles[y + 1][x].passages[0] = FixConnection(Tiles[y][x].passages[2], Tiles[y + 1][x].passages[0]);
                }
                if (Tiles[y][x].passages[1] != Tiles[y][x + 1].passages[3])
                {
                    Tiles[y][x].passages[1] = FixConnection(Tiles[y][x].passages[1], Tiles[y][x + 1].passages[3]);
                    Tiles[y][x + 1].passages[3] = FixConnection(Tiles[y][x].passages[1], Tiles[y][x + 1].passages[3]);
                }
            }
        }
        for (int x = 0; x < Width; x++)
        {
            Tiles[0][x].passages[0] = PassageType.No;
        }
        for (int y = 0; y < Height; y++)
        {
            Tiles[y][Width-1].passages[1] = PassageType.No;
        }
        for (int x = 0; x < Width; x++)
        {
            Tiles[Height-1][x].passages[2] = PassageType.No;
        }
        for (int y = 0; y < Height; y++)
        {
            Tiles[y][0].passages[3] = PassageType.No;
        }
    }

    PassageType FixConnection(PassageType pass1, PassageType pass2)
    {
        if ((pass1 == PassageType.No) || (pass2 == PassageType.No))
        {
            return PassageType.No;
        }
        if((pass1 == PassageType.SecretDoor) || (pass2 == PassageType.SecretDoor))
        {
            return PassageType.SecretDoor;
        }
        if ((pass1 == PassageType.Door) || (pass2 == PassageType.Door))
        {
            return PassageType.Door;
        }
        return PassageType.Corridor;
    }
    void GeneratePlatforms(int x, int y, List<List<BiomeTilesData>> BiomePrefabs)
    {
        for (int i = 0; i < BiomePrefabs[(int)Tiles[y][x].biome1][(int)Tiles[y][x].biome2].PlatformPrefab.Count; i++)
        {
            if (!BiomePrefabs[(int)Tiles[y][x].biome1][(int)Tiles[y][x].biome2].PlatformPrefab[i].GetComponent<Platform>().sub)
            {
                Tiles[y][x].TilePlatforms.AddRange(BiomePrefabs[(int)Tiles[y][x].biome1][(int)Tiles[y][x].biome2].PlatformPrefab[i].GetComponent<Platform>().GeneratePlatforms());
            }
        }
    }

    void AdjustPlatformPositions(int x, int y)
    {
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
            Tiles[y][x].TilePlatforms[i].location = new SVector3(Tiles[y][x].TilePlatforms[i].location.x,
                                                                 Tiles[y][x].TilePlatforms[i].location.y + (4-j)*1.5f,
                                                                 Tiles[y][x].TilePlatforms[i].location.z + 0.0f);
        }
    }

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
    void PrintTileToFile(List<List<int>> Tiles, int x1, int y1, bool noPass = false)
    {
        string name;
        if (noPass)
        {
            name = "Tiles\\TestTile"+y1.ToString()+x1.ToString()+"1.txt";
        }
        else
        {
            name = "Tiles\\TestTile" + y1.ToString() + x1.ToString() + ".txt";
        }
        var sr = File.CreateText(name);
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 50; x++)
            {
                switch (Tiles[y][x])
                {
                    case 0:
                        sr.Write("=");// 
                        break;
                    case 1:
                        sr.Write("/");// 
                        break;
                    case 2:
                        sr.Write("\\");// 
                        break;
                    case 3:
                        sr.Write("#");// 
                        break;
                    default:
                        break;
                }
            }
            sr.WriteLine();
        }
        sr.Close();
    }
    void PrintMapToFile(bool noPass = false)
	{
        string name;
        if (noPass)
        {
            name = "TestMap1.txt";
        }
        else
        {
            name = "TestMap.txt";
        }
		var sr = File.CreateText(name);
		for (int y = 0; y < Height; y++)
		{
            if (!noPass)
            {
                for (int x = 0; x < Width; x++)
                {
                    switch (Tiles[y][x].passages[0])
                    {
                        case PassageType.No:
                            sr.Write("  ");// 
                            break;
                        case PassageType.Door:
                            sr.Write(" |");// 
                            break;
                        case PassageType.SecretDoor:
                            sr.Write(" :");// 
                            break;
                        case PassageType.Corridor:
                            sr.Write(" H");// 
                            break;
                        default:
                            break;
                    }
                }
                sr.WriteLine();
            }
			for (int x = 0; x < Width; x++)
			{
                if (!noPass)
                {
                    switch (Tiles[y][x].passages[3])
                    {
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
				switch (Tiles [y] [x].biome1)
				{
					case BiomesTypes.Forest:
						sr.Write ('T');
						break;
					case BiomesTypes.Dump:
						sr.Write ('%');
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
					case BiomesTypes.Waterfall:
						sr.Write ('|');
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
					case BiomesTypes.Shroom:
						sr.Write ('P');
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
					case BiomesTypes.River:
						sr.Write ('~');
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
					case BiomesTypes.Caves:
						sr.Write ('O');
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
					case BiomesTypes.Swamp:
						sr.Write ('x');
						break;
					/*case BiomesTypes.SwampMill:
						sr.Write ('y');
						break;
					case BiomesTypes.SwampMaze:
						sr.Write ('W');
						break;*/
					case BiomesTypes.Mill:
						sr.Write ('A');
						break;
					/*case BiomesTypes.MillMaze:
						sr.Write ('H');
						break;*/
					case BiomesTypes.Maze:
						sr.Write ('#');
						break;
					default:
						sr.Write ('.');
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
			sr.WriteLine ();
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
}
