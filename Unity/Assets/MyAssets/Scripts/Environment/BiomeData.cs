using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.Entities;
using UnityEngine;
[Serializable]
public enum BiomesTypes { Forest, Dump, Waterfall, Shroom, River, Caves, Swamp, Mill, Maze };//{ Forest, Dump, DumpWaterfall, DumpShroom, DumpRiver, DumpCaves, DumpSwamp, DumpMill, DumpMaze, Waterfall, WaterfallShroom, WaterfallRiver, WaterfallCaves, WaterfallSwamp, WaterfallMill, WaterfallMaze, Shroom, ShroomRiver, ShroomCaves, ShroomSwamp, ShroomMill, ShroomMaze, River, RiverCaves, RiverSwamp, RiverMill, RiverMaze, Caves, CavesSwamp, CavesMill, CavesMaze, Swamp, SwampMill, SwampMaze, Mill, MillMaze, Maze };

public enum MaxBiomeSizeInPercents { Enormous = 100, Large = 50, Medium = 30, Small = 10 };

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class BiomeData: ScriptableObject
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
    public List<AbstractEntity> EntitiesPrefabs = new List<AbstractEntity>();
    public List<int> EntitiesAmounts = new List<int>();
    public Chest ChestPrefab;
    public List<GameObject> BlockPrefabs = new List<GameObject>();
}
