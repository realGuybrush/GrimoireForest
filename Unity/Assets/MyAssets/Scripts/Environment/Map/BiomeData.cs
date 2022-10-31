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
}

[Serializable]
public class ListBiomeData {
    public List<BiomeData> list = new List<BiomeData>();
}
