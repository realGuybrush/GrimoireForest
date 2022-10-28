using System.Collections.Generic;
using UnityEngine;
using MyAssets.Scripts.Environment;

public partial class WorldManagement : MonoBehaviour {

    [SerializeField]
    private Tile tilePrefab;

    private List<Tile> currentlySpawnedTiles = new List<Tile>();

    public float TileWidth = 80.5f;

    public EnvironmentFactory environmentFactory; //todo: make private

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

    public void SpawnCorridor(DirectionType LookTurn = DirectionType.North, bool load = false) {
        int xCoeff = calculateXDirectionCoeff(LookTurn);
        int yCoeff = -calculateYDirectionCoeff(LookTurn);
        int offsetTilesLeft = 0;
        int offsetTilesRight = 0;
        while (GlobalMap.Tiles[PlayerYMap + offsetTilesLeft * xCoeff][PlayerXMap + offsetTilesLeft * yCoeff]
            .passages[3] == PassageType.Corridor) {
            offsetTilesLeft--;
        }
        while (GlobalMap.Tiles[PlayerYMap + offsetTilesLeft * xCoeff][PlayerXMap + offsetTilesRight * yCoeff]
            .passages[1] == PassageType.Corridor) {
            offsetTilesRight++;
        }
        for (int i = offsetTilesLeft; i <= offsetTilesRight; i++) {
            SpawnTile(i, GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff], LookTurn, load);
            int j = GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].TileEntitiesPositions.Count - 1;
            Environment.transform.GetChild(i - offsetTilesLeft + 5).gameObject.GetComponent<AdjustBGY>()
                .AdjustBGPartPositions(GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].blocks, 4);
            Environment.transform.GetChild(i - offsetTilesLeft + 5).gameObject.GetComponent<AdjustBGY>()
                .AdjustBGPartPositions(GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].blocks, 5);
            if ((GlobalTalks.Count > j) && (j != -1)) {
                while (GlobalTalks[j].TileCoords.x < 0) {
                    GlobalTalks[j].TileCoords.x = PlayerXMap + i * yCoeff;
                    GlobalTalks[j].TileCoords.y = PlayerYMap + i * xCoeff;
                    j--;
                }
            }
        }
        GameObject.Find("Main Camera").GetComponent<Camera_Movement>()
            .SetCameraBoundaries(offsetTilesLeft, offsetTilesRight);
    }

    private void SpawnTile(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North,
        bool load = false) {
            currentlySpawnedTiles.Add(Instantiate(tilePrefab,
                new Vector3(xTileOffset * TileWidth, 0.0f, 0.0f), new Quaternion(), Environment.transform));
            currentlySpawnedTiles[currentlySpawnedTiles.Count-1].Init(xTileOffset, MT, LookTurn, load);
    }

    public bool CoordsInBiomeCenters(int x, int y) {
        for (int i = 0; i < GlobalMap.Biomes.Count; i++) {
            if ((GlobalMap.Biomes[i].Center.x == x) && (GlobalMap.Biomes[i].Center.y == y)) {
                return true;
            }
        }
        return false;
    }

    public void SetGlobalEntities() {
        int x, y;
        for (int i = 0; i < GlobalEntities.Count; i++) {
            x = Random.Range(0, GlobalMap.Width);
            y = Random.Range(0, GlobalMap.Height);
            while (CoordsInBiomeCenters(x, y)) {
                x = Random.Range(0, GlobalMap.Width);
                y = Random.Range(0, GlobalMap.Height);
            }
            GlobalMap.Tiles[y][x].TileEntitiesPositions.Add(new EntityValues(GlobalEntities[i],
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

    public void MapGeneration() {
        GlobalMap.Generate_Map();
        PlayerXMap = (int) GlobalMap.Biomes[0].Center.x;
        PlayerYMap = (int) GlobalMap.Biomes[0].Center.y;
        SetGlobalEntities();
        SpawnCorridor(DirectionType.North);
        Player.GetComponent<PlayerControls>().ShowHideMenu();
        Player.transform.position = new Vector3(0.0f, 0.0f, Player.transform.position.z);
    }
}
