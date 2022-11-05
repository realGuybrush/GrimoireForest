using System.Collections.Generic;
using UnityEngine;
using MyAssets.Scripts.Environment;

public partial class WorldManagement {

    [SerializeField]
    private Tile tilePrefab;

    [SerializeField]
    private float tileBlockWidth = 1.5f;

    private List<Tile> currentlySpawnedTiles = new List<Tile>();

    public EnvironmentFactory environmentFactory; //todo: make private

    public void MapGeneration() {
        globalMap.Generate_Map();
        PlayerXMap = (int) globalMap.MapCenter.x;
        PlayerYMap = (int) globalMap.MapCenter.y;
        SetGlobalEntities();
        SpawnCorridor(DirectionType.North);
        Player.GetComponent<PlayerControls>().ShowHideMenu();
        Player.transform.position = new Vector3(0.0f, 0.0f, Player.transform.position.z);
    }

    public void SpawnCorridor(DirectionType LookTurn = DirectionType.North, bool load = false) {
        int xCoeff = calculateXDirectionCoeff(LookTurn);
        int yCoeff = -calculateYDirectionCoeff(LookTurn);
        int offsetTilesLeft = 0;
        int offsetTilesRight = 0;
        while (globalMap.Tiles[PlayerYMap + offsetTilesLeft * xCoeff][PlayerXMap + offsetTilesLeft * yCoeff]
            .passages[3] == PassageType.Corridor) {
            offsetTilesLeft--;
        }
        while (globalMap.Tiles[PlayerYMap + offsetTilesLeft * xCoeff][PlayerXMap + offsetTilesRight * yCoeff]
            .passages[1] == PassageType.Corridor) {
            offsetTilesRight++;
        }
        for (int i = offsetTilesLeft; i <= offsetTilesRight; i++) {
            SpawnTile(i, globalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff], LookTurn, load);
            int j = globalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].TileEntitiesPositions.Count - 1;
            Environment.transform.GetChild(i - offsetTilesLeft + 5).gameObject.GetComponent<AdjustBGY>()
                .AdjustBGPartPositions(globalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].blocks, 4);
            Environment.transform.GetChild(i - offsetTilesLeft + 5).gameObject.GetComponent<AdjustBGY>()
                .AdjustBGPartPositions(globalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].blocks, 5);
            if ((GlobalTalks.Count > j) && (j != -1)) {
                while (GlobalTalks[j].TileCoords.x < 0) {
                    GlobalTalks[j].TileCoords.x = PlayerXMap + i * yCoeff;
                    GlobalTalks[j].TileCoords.y = PlayerYMap + i * xCoeff;
                    j--;
                }
            }
        }
        mainCamera.SetCameraBoundaries(offsetTilesLeft, offsetTilesRight);
    }

    private void SpawnTile(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North,
        bool load = false) {
            currentlySpawnedTiles.Add(Instantiate(tilePrefab,
                new Vector3(xTileOffset, 0.0f, 0.0f), new Quaternion(), Environment.transform));
            currentlySpawnedTiles[currentlySpawnedTiles.Count-1].Init(xTileOffset, MT, LookTurn, load);
    }

    public void DeleteCorridor() {
        for (int j = 0; j < 5; j++)
        for (int i = Environment.transform.GetChild(j).transform.childCount - 1; i > -1; i--) {
            Destroy(Environment.transform.GetChild(j).transform.GetChild(i).gameObject);
        }
        for (int i = Environment.transform.childCount - 1; i > 4; i--) {
            Destroy(Environment.transform.GetChild(i).gameObject);
        }
        Player.GetComponent<PlayerControls>().RemoveDoors();
    }

    public bool CoordsInBiomeCenters(int x, int y) {
        for (int i = 0; i < globalMap.Biomes.Count; i++) {
            if ((globalMap.Biomes[i].Center.x == x) && (globalMap.Biomes[i].Center.y == y)) {
                return true;
            }
        }
        return false;
    }

    public void SetGlobalEntities() {
        int x, y;
        for (int i = 0; i < GlobalEntities.Count; i++) {
            x = Random.Range(0, globalMap.Width);
            y = Random.Range(0, globalMap.Height);
            while (CoordsInBiomeCenters(x, y)) {
                x = Random.Range(0, globalMap.Width);
                y = Random.Range(0, globalMap.Height);
            }
            globalMap.Tiles[y][x].TileEntitiesPositions.Add(new EntityValues(GlobalEntities[i],
                new Vector3(Random.Range(-TileWidth / 2, TileWidth / 2), 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Inventory(), //EntityPrefabs[BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<BasicMovement>().inventory
                new Characteristics())); //EntityPrefabs[BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<BasicMovement>().thisHealth.values
        }
    }

    public void UpdateGlobalTalks(int index, Vector3 coord, Talk talk) {
        for (int i = 0; i < GlobalTalks.Count; i++) {
            if ((GlobalTalks[i].entityIndexOnTile == index) && (GlobalTalks[i].TileCoords == coord)) {
                GlobalTalks[i].talk = talk;
            }
        }
    }
}
