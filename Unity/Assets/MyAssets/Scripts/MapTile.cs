using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassageType : int { No, Door, Corridor, SecretDoor };

public enum DirectionType : int { North, East, South, West };


public class MapTile
{
    public BiomesTypes biome1 = BiomesTypes.Forest;
    public BiomesTypes biome2 = BiomesTypes.Forest;
    public List<GameObject> TileEntities = new List<GameObject>();
    public List<GameObject> TileChests = new List<GameObject>();
    public List<GameObject> TileBarrcicades = new List<GameObject>();
    //n e s w
    public List<PassageType> passages = new List<PassageType>() { PassageType.No, PassageType.No, PassageType.No, PassageType.No };
}
