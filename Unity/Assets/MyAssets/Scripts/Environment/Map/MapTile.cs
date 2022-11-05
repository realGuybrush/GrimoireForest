using System;
using System.Collections.Generic;
using MyAssets.Scripts;
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
    IncLeftGrassOnly,
    IncRightBushU,
    IncRightBushD,
    IncRightGrassOnly,
    DoorD,
    DoorULeftOpen,
    DoorULeftClosed,
    DoorURightOpen,
    DoorURightClosed
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
    private int distanceFromSideDoorToEdge = 2;

    public BiomeType biome1 = BiomeType.Forest;
    public BiomeType biome2 = BiomeType.Forest;

    public InhabitationPattern spawnPattern = new InhabitationPattern();

    //todo: replace with grid
    public List<List<BlockType>> blocks = new List<List<BlockType>>();
    public List<List<BlockType>> bushes = new List<List<BlockType>>();
    public List<EntityValues> TileEntitiesPositions = new List<EntityValues>();
    public List<EnvironmentStuffingValues> TileChests = new List<EnvironmentStuffingValues>();
    public List<Inventory> TileChestsInventry = new List<Inventory>();
    public List<EnvironmentStuffingValues> TileBarricades = new List<EnvironmentStuffingValues>();
    public List<EnvironmentStuffingValues> TilePlatforms = new List<EnvironmentStuffingValues>();

    public List<EnvironmentStuffingValues> TileItems = new List<EnvironmentStuffingValues>();

    //n e s w
    public List<PassageType> passages = new List<PassageType>()
        {PassageType.No, PassageType.No, PassageType.No, PassageType.No};

    private int gridHeight;
    private int gridWidth;
    private int doorWidth;

    public void Init(int GridWidth = 43, int GridHeight = 9, int DoorWidth = 6) {
        environmentFactory = EnvironmentFactory.GetInstance;
        gridWidth = GridWidth;
        gridHeight = GridHeight;
        doorWidth = DoorWidth;
        GenerateTileStructure();
    }

    public void GenerateTileStructure() {
        environmentFactory.GenerateGroundByBiome(biome1, biome2, blocks, gridWidth, gridHeight);
        if (gridWidth != blocks[0].Count) gridWidth = blocks[0].Count;
        if (gridHeight != blocks.Count) gridHeight = blocks.Count;
        SetBushes();
        SetDoors();
    }

    private void SetBushes() {
        VASUtilities<BlockType>.SetDefaultValuesSquareList(bushes, gridWidth, gridHeight, BlockType.Empty);
        for (int y = 1; y < gridHeight; y++)
        for (int x = 0; x < gridWidth; x++) {
            if (blocks[y][x] == BlockType.TopBushU)
                bushes[y - 1][x] = BlockType.TopBushD;
            else if (blocks[y][x] == BlockType.IncLeftBushU)
                bushes[y - 1][x] = BlockType.IncLeftBushD;
            else if (blocks[y][x] == BlockType.IncRightBushU)
                bushes[y - 1][x] = BlockType.IncRightBushD;
        }
    }

    private void SetDoors() {
        //todo: I'm almost totally sure there is a better way to do this
        int y;
        if (gridWidth > distanceFromSideDoorToEdge + 1) {
            if (passages[(int) DirectionType.East] == PassageType.Door) {
                y = GetColumnTopPosition(gridWidth - distanceFromSideDoorToEdge - 1);
                blocks[y][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.DoorURightOpen;
                blocks[y - 1][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.NoTop;
                bushes[y - 1][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.DoorD;
            } else {
                if (passages[(int) DirectionType.East] == PassageType.No) {
                    y = GetColumnTopPosition(gridWidth - distanceFromSideDoorToEdge - 1);
                    blocks[y][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.DoorURightClosed;
                    blocks[y - 1][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.NoTop;
                    bushes[y - 1][gridWidth - distanceFromSideDoorToEdge - 1] = BlockType.DoorD;
                }
            }
            if (passages[(int) DirectionType.West] == PassageType.Door) {
                y = GetColumnTopPosition(distanceFromSideDoorToEdge);
                blocks[y][distanceFromSideDoorToEdge] = BlockType.DoorULeftOpen;
                blocks[y - 1][distanceFromSideDoorToEdge] = BlockType.NoTop;
                bushes[y - 1][distanceFromSideDoorToEdge] = BlockType.DoorD;
            } else {
                if (passages[(int) DirectionType.West] == PassageType.No) {
                    y = GetColumnTopPosition(distanceFromSideDoorToEdge);
                    blocks[y][distanceFromSideDoorToEdge] = BlockType.DoorULeftClosed;
                    blocks[y - 1][distanceFromSideDoorToEdge] = BlockType.NoTop;
                    bushes[y - 1][distanceFromSideDoorToEdge] = BlockType.DoorD;
                }
            }
        }
        if (passages[(int) DirectionType.North] != PassageType.No ||
            passages[(int) DirectionType.South] != PassageType.No) {
            for (int x = (gridWidth - doorWidth) / 2; x < (gridWidth + doorWidth) / 2; x++) {
                y = GetColumnTopPosition(x);
                if (y > 0) {
                    if (passages[(int) DirectionType.South] == PassageType.Door)
                        bushes[y - 1][x] = BlockType.Empty;
                    if (passages[(int) DirectionType.North] == PassageType.Door)
                        if (blocks[y][x] == BlockType.TopBushU) {
                            blocks[y][x] = BlockType.Top;
                        } else if (blocks[y][x] == BlockType.IncLeftBushU) {
                            blocks[y][x] = BlockType.IncLeftGrassOnly;
                        } else if (blocks[y][x] == BlockType.IncRightBushU) {
                            blocks[y][x] = BlockType.IncRightGrassOnly;
                        }
                }
            }
        }
    }

    public int GetColumnTopPosition(int x) {
        for (int y = 0; y < blocks.Count; y++) {
            if (blocks[y][x] == BlockType.Top || blocks[y][x] == BlockType.TopBushU ||
                blocks[y][x] == BlockType.IncLeftGrassOnly || blocks[y][x] == BlockType.IncLeftBushU ||
                blocks[y][x] == BlockType.IncRightGrassOnly || blocks[y][x] == BlockType.IncRightBushU)
                return y;
        }
        return blocks.Count;
    }

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

    public int GridHeight => gridHeight;
    public int GridWidth => gridWidth;

}
