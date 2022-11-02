using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.Entities;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum BiomeType { Forest, Dump, Waterfall, Shroom, River, Caves, Swamp, Mill, Maze };//{ Forest, Dump, DumpWaterfall, DumpShroom, DumpRiver, DumpCaves, DumpSwamp, DumpMill, DumpMaze, Waterfall, WaterfallShroom, WaterfallRiver, WaterfallCaves, WaterfallSwamp, WaterfallMill, WaterfallMaze, Shroom, ShroomRiver, ShroomCaves, ShroomSwamp, ShroomMill, ShroomMaze, River, RiverCaves, RiverSwamp, RiverMill, RiverMaze, Caves, CavesSwamp, CavesMill, CavesMaze, Swamp, SwampMill, SwampMaze, Mill, MillMaze, Maze };

public enum MaxBiomeSizeInPercents { Enormous = 100, Large = 50, Medium = 30, Small = 10, Tiny = 2 };//todo: maybe replace with float

[CreateAssetMenu(fileName = "Biome", menuName = "ScriptableObjects/BiomeDTO", order = 1)]
public class BiomeData: ScriptableObject
{
    public bool aggressive;
    public bool immortal;
    //todo: make growthPower somehow adjustable, so that it could change throughout the game based on events;
    //it must not change default values, so that at new game start default values would always be the same;
    //maybe make here a property, which would ask Save about some completed events to return adjusted value
    public int growthPower;
    public int resistancePower;
    public MaxBiomeSizeInPercents minPercentSize;
    public MaxBiomeSizeInPercents maxPercentSize;
    public Sprite Sky;
    public Sprite Moon;
    public List<GameObject> PlatformPrefab;
    public List<Cover> CoverPrefabs;
    public List<EnemyMovement> EntitiesPrefabs;
    public List<int> EntitiesAmounts;
    public Chest ChestPrefab;
    public SerializableDictionaryBase<BlockType, TileBase> BlockPrefabs;

    [SerializeField]
    protected int maxWaveHeight = 1, maxWaveInstantChange = 1, defaultWaveChance = 50;

    public virtual void GenerateGround(List<List<BlockType>> blocks, int gridWidth, int gridHeight) {
        //remark: I know that it's wrong to send data of one class to another to edit,
        //        but it is the only way I came up with to make ground generation in Biome, not MapTile
        for (int i = 0; i < gridHeight / 2; i++) {
            blocks.Add(new List<BlockType>());
            for (int j = 0; j < gridWidth; j++)
                blocks[i].Add(BlockType.NoTop);
        }
        blocks.Add(new List<BlockType>());
        for (int j = 0; j < gridWidth; j++)
            blocks[gridHeight / 2].Add( BlockType.Top);
        for (int i = gridHeight / 2 + 1; i < gridHeight; i++) {
            blocks.Add(new List<BlockType>());
            for (int j = 0; j < gridWidth; j++)
                blocks[i].Add(BlockType.Empty);
        }
    }

    /*private void SetBushesDoors() {
        for (int i = blocks[0].Count / 2 - 4; i < blocks[0].Count / 2 + 3; i++) {
            for (int j = 0; j < gridHeight; j++) {
                if (blocks[j][i] ==  BlockType.TopBushUD) {
                    if ((j == 0) || (blocks[j - 1][i] == BlockType.Empty)) {
                        if (passages[0] == PassageType.Door) {
                            if (passages[2] == PassageType.Door) {
                                blocks[j][i] = BlockType.Top;
                            } else {
                                blocks[j][i] = BlockType.TopBushD;
                            }
                        } else {
                            if (passages[2] == PassageType.Door) {
                                blocks[j][i] = BlockType.TopBushU;
                            } else {
                                blocks[j][i] = BlockType.TopBushUD;
                            }
                        }
                    }
                }
                if (blocks[j][i] == BlockType.IncLeftBushUD) {
                    if (passages[0] == PassageType.Door) {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = BlockType.InclLeft;
                        } else {
                            blocks[j][i] = BlockType.IncLeftBushD;
                        }
                    } else {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = BlockType.IncLeftBushU;
                        } else {
                            blocks[j][i] = BlockType.IncLeftBushUD;
                        }
                    }
                }
                if (blocks[j][i] == BlockType.IncRightBushUD) {
                    if (passages[0] == PassageType.Door) {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = BlockType.InclRight;
                        } else {
                            blocks[j][i] = BlockType.IncRightBushD;
                        }
                    } else {
                        if (passages[2] == PassageType.Door) {
                            blocks[j][i] = BlockType.IncRightBushU;
                        } else {
                            blocks[j][i] = BlockType.IncRightBushUD;
                        }
                    }
                }
            }
        }
    }*/

    //void GeneratePlatforms() {
    //    for (int i = 0; i < environmentFactory.GetBiomePlatformCount(biome1, biome2); i++) {
            //todo: add platform spawning
            /*if (!BiomePrefabs[(int) Tiles[y][x].biome1][(int) Tiles[y][x].biome2].PlatformPrefab[i]
                .GetComponent<Platform>().sub) {
                Tiles[y][x].TilePlatforms.AddRange(BiomePrefabs[(int) Tiles[y][x].biome1][(int) Tiles[y][x].biome2]
                    .PlatformPrefab[i].GetComponent<Platform>().GeneratePlatforms(Tiles[y][x].blocks));
            }*/
    //    }
    //}

    /*void AdjustPlatformPositions() {
        int midlevel = gridHeight/2;
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
            for (j = 0; j < gridHeight; j++) {
                if (blocks[j][k] != BlockType.Empty)
                    break;
            }
            TilePlatforms[i].location = new SVector3(TilePlatforms[i].location.x,
                TilePlatforms[i].location.y + (midlevel - j - inclinedOffsety) * 1.5f,
                TilePlatforms[i].location.z + 0.0f);
        }
    }*/ /*
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
}

[Serializable]
public class ListBiomeData {
    public List<BiomeData> list = new List<BiomeData>();
}
