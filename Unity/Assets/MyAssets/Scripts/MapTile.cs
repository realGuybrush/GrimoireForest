using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum PassageType : int { No, Door, Corridor, SecretDoor };

public enum DirectionType : int { North, East, South, West };
public enum BlockType : int { Top, InclLeft, InclRight, NoTop, Empty, TopBushU, TopBushD, TopBushUD, IncLeftBushU, IncLeftBushD, IncLeftBushUD, IncRightBushU, IncRightBushD, IncRightBushUD };

[Serializable]
public class EnvironmentStuffingValues
    {
    public SVector3 location = new SVector3();
    public int indexInPrefabs = 0;
    public EnvironmentStuffingValues(Vector3 v, int i)
    {
        location = new SVector3(v);
        indexInPrefabs = i;
    }
}
[Serializable]
public class EntityValues
{
    public int indexInPrefabs = 0;
    public SVector3 location = new SVector3();
    public SVector3 rotation = new SVector3();
    public SVector3 velocity = new SVector3();
    public Inventory inventory = new Inventory();
    public Characteristics characteristics = new Characteristics();
    public EntityValues(int i, Vector3 l, Vector3 r, Vector3 v, Inventory I, Characteristics c)
    {
        indexInPrefabs = i;
        location = new SVector3(l);
        rotation = new SVector3(r);
        velocity = new SVector3(v);
        inventory = I;
    }
}
[Serializable]
public class InhabitationPattern
{
    public bool spawnOnce = true;
    public List<int> amounts = new List<int>();
    public List<int> entityPrebabIndexes = new List<int>();
}

[Serializable]
public class MapTile
{
    public BiomesTypes biome1 = BiomesTypes.Forest;
    public BiomesTypes biome2 = BiomesTypes.Forest;
    public InhabitationPattern spawnPattern = new InhabitationPattern();
    public List<List<int>> blocks = new List<List<int>>();
    public List<EntityValues> TileEntitiesPositions = new List<EntityValues>();
    public List<EnvironmentStuffingValues> TileChests = new List<EnvironmentStuffingValues>();
    public List<Inventory> TileChestsInventry = new List<Inventory>();
    public List<EnvironmentStuffingValues> TileBarricades = new List<EnvironmentStuffingValues>();
    public List<EnvironmentStuffingValues> TilePlatforms = new List<EnvironmentStuffingValues>();
    public List<EnvironmentStuffingValues> TileItems = new List<EnvironmentStuffingValues>();
    //n e s w
    public List<PassageType> passages = new List<PassageType>() { PassageType.No, PassageType.No, PassageType.No, PassageType.No };
}
