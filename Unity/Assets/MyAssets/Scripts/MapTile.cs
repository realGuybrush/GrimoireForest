using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomesTypes : int { Forest, Dump, DumpWaterfall, DumpShroom, DumpRiver, DumpCaves, DumpSwamp, DumpMill, DumpMaze, Waterfall, WaterfallShroom, WaterfallRiver, WaterfallCaves, WaterfallSwamp, WaterfallMill, WaterfallMaze, Shroom, ShroomRiver, ShroomCaves, ShroomSwamp, ShroomMill, ShroomMaze, River, RiverCaves, RiverSwamp, RiverMill, RiverMaze, Caves, CavesSwamp, CavesMill, CavesMaze, Swamp, SwampMill, SwampMaze, Mill, MillMaze, Maze };

public enum PassageType : int { No, Door, Corridor, SecretDoor };

public enum DirectionType : int { No, North, East, South, West };

public class Biome
{
    public BiomesTypes id;
    public Vector3 Center = new Vector3();
    public int radius;
    public Biome(BiomesTypes ID)
    {
        id = ID;
    }
}

[System.Serializable]
public class MapTile
{
    public BiomesTypes biome = BiomesTypes.Forest;
    public int FrontalBackgroundIndex;//probably, replace it with object
    public int RoadIndex;//fix: the same
    public List<GameObject> TileBackgroundObjects = new List<GameObject>();
    public List<GameObject> TileBarrcicades = new List<GameObject>();
    public PassageType northern_passage = PassageType.No;
    public PassageType southern_passage = PassageType.No;
    public PassageType eastern_passage = PassageType.No;
    public PassageType western_passage = PassageType.No;
}
