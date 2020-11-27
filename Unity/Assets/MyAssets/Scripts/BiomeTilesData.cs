using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum BiomesTypes : int { Forest, Dump, Waterfall, Shroom, River, Caves, Swamp, Mill, Maze };//{ Forest, Dump, DumpWaterfall, DumpShroom, DumpRiver, DumpCaves, DumpSwamp, DumpMill, DumpMaze, Waterfall, WaterfallShroom, WaterfallRiver, WaterfallCaves, WaterfallSwamp, WaterfallMill, WaterfallMaze, Shroom, ShroomRiver, ShroomCaves, ShroomSwamp, ShroomMill, ShroomMaze, River, RiverCaves, RiverSwamp, RiverMill, RiverMaze, Caves, CavesSwamp, CavesMill, CavesMaze, Swamp, SwampMill, SwampMaze, Mill, MillMaze, Maze };

public enum MaxBiomeSizeInPercents : int { Enormous = 100, Large = 50, Medium = 30, Small = 10 };

[Serializable]
public class Biome
{
    public BiomesTypes id;
    public SVector3 Center = new SVector3();
    public int radius;
    public Biome(BiomesTypes ID)
    {
        id = ID;
    }
}
public class BiomeTilesData
{
    public static int BiomesAmount = 1;
    public bool aggressive;
    public bool immortal;
    public MaxBiomeSizeInPercents Size;
    public Sprite Sky;
    public Sprite Moon;
    public GameObject TilePrefab;
    public List<GameObject> PlatformPrefab = new List<GameObject>();
    public List<GameObject> CoverPrefabs = new List<GameObject>();
    public List<int> EntitiesPrefabs = new List<int>();
    public List<int> EntitiesAmounts = new List<int>();
    public GameObject ChestPrefab;
    public List<GameObject> BlockPrefabs = new List<GameObject>();
}
