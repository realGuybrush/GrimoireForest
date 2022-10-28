using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MyAssets.Scripts.Environment;

public partial class WorldManagement : MonoBehaviour {

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

    public void SpawnTile(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North,
        bool load = false) {
        GameObject GO =
            GameObject.Instantiate(environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2].TilePrefab,
                new Vector3(xTileOffset * TileWidth, 0.0f, 0.0f), new Quaternion(), Environment.transform);
        //GameObject GO2 = GameObject.Instantiate(BiomePrefabs[(int)type1][(int)type2].PlatformPrefab);//print them all, probably set on first spawn
        //make sky and moon placemens as part of Update and put it on movement through various biomes
        //-3 -2 -1 1 R L G
        //wall path
        GO.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset =
            xTileOffset;
        GO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject
            .GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset =
            xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset =
            xTileOffset;
        if (MT.passages[(0 + (int) LookTurn) % 4] == PassageType.Door) {
            GO.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
            GO.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
        } else {
            GO.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
            GO.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        }

        if ((MT.passages[(1 + (int) LookTurn) % 4] != PassageType.No) &&
            (MT.passages[(1 + (int) LookTurn) % 4] != PassageType.SecretDoor)) {
            GO.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
        }
        if (MT.passages[(1 + (int) LookTurn) % 4] != PassageType.Door) {
            GO.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
        }

        if (MT.passages[(2 + (int) LookTurn) % 4] == PassageType.Door) {
            GO.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
        } else {
            GO.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
        }

        if ((MT.passages[(3 + (int) LookTurn) % 4] != PassageType.No) &&
            (MT.passages[(3 + (int) LookTurn) % 4] != PassageType.SecretDoor)) {
            GO.transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
        }
        if (MT.passages[(3 + (int) LookTurn) % 4] != PassageType.Door) {
            GO.transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < GlobalMap.BlocksInTile; j++) {
                if (MT.blocks[i][j] != (int) BlockType.Empty)
                    Instantiate(
                        environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2].BlockPrefabs[MT.blocks[i][j]],
                        new Vector3(
                            (j - GlobalMap.BlocksInTile / 2) * 1.5f + xTileOffset * (GlobalMap.BlocksInTile * 1.5f),
                            (i) * -1.5f, 0.0f), new Quaternion(), GO.transform.GetChild(6));
            }
        }

        for (int i = 0; i < MT.TilePlatforms.Count; i++) {
            Instantiate(environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2], MT.TilePlatforms[i],
                Environment.transform.GetChild(0).transform);
        }
        for (int i = 0; i < MT.TileChests.Count; i++) {
            SpawnChest(MT, i);
        }
        for (int i = 0; i < MT.TileItems.Count; i++) {
            Drop(MT.TileItems[i].indexInPrefabs, 1, MT.TileItems[i].location.ToV3());
        }
        if ((MT.TileEntitiesPositions.Count == 0) && !load) {
            SetTileEntities(MT);
        }
        for (int i = 0; i < MT.TileEntitiesPositions.Count; i++) {
            SpawnEntity(MT.TileEntitiesPositions[i], i);
        }
    }

    void SetTileEntities(MapTile MT) {
        //todo: fix this crap; maybe remove this class altogether
        int newAmount;
        //fix create here array of entities in tile to spawn them
        for (int i = 0; i < environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2].EntitiesPrefabs.Count; i++) {
            newAmount = Random.Range(0,
                environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2].EntitiesAmounts[i]);
            /*if (EntityPrefabs[environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<NPCBehaviour>() != null)
            {
                newAmount = 1;
                GlobalTalks.Add(new TalkData(MT.TileEntitiesPositions.Count, environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i], new Vector3(-1, -1, 0),
                    EntityPrefabs[environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<NPCBehaviour>().talk));
            }
            for (int j = 0; j < newAmount; j++)
            {
                MT.TileEntitiesPositions.Add(new EntityValues(environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i],
                                                              new Vector3(Random.Range((float)(-TileWidth / 2), (float)(TileWidth / 2)), 0.0f, 0.0f),
                                                              new Vector3(0.0f, 0.0f, 0.0f),
                                                              new Vector3(0.0f, 0.0f, 0.0f),
                                                              new Inventory(),
                                                              new Characteristics()));
            }*/
        }
    }

    void Instantiate(BiomeData BTD, EnvironmentStuffingValues ESV, Transform parent) {
        try {
            GameObject.Instantiate(BTD.PlatformPrefab[ESV.indexInPrefabs], ESV.location.ToV3(), new Quaternion(),
                parent);
        } catch {
            Debug.Log(ESV.indexInPrefabs);
        }
    }

    public GameObject SpawnEntity(EntityValues EV, int parentIndex = -1) {
        GameObject entity =
            GameObject.Instantiate(EntityPrefabs[EV.indexInPrefabs], GameObject.Find("Entities").transform);
        entity.transform.position = EV.location.ToV3();
        entity.transform.eulerAngles = EV.rotation.ToV3();
        entity.GetComponent<Rigidbody2D>().velocity = EV.velocity.ToV3();
        entity.GetComponent<BasicMovement>().inventory = EV.inventory;
        entity.GetComponent<BasicMovement>().thisHealth.values = EV.characteristics;
        if (entity.transform.GetChild(entity.transform.childCount - 1).name == "Attached") {
            for (int i = 0; i < entity.transform.GetChild(entity.transform.childCount - 1).childCount; i++) {
                entity.transform.GetChild(entity.transform.childCount - 1).GetChild(i).GetComponent<EnemyMovement>()
                    .attachedTo = parentIndex;
            }
        }
        return entity;
    }

    public void SpawnChest(MapTile MT, int i) {
        GameObject temp;
        if (MT.TileChests[i].indexInPrefabs == 0) {
            temp = GameObject.Instantiate(DropPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(),
                Environment.transform.GetChild(3).transform);
            temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
        } else {
            //todo: fix this crap
            //temp = GameObject.Instantiate(environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].ChestPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(), Environment.transform.GetChild(3).transform);
            //temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
        }
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
        GlobalMap.Generate_Map(environmentFactory.BiomeDTOs[0]);
        PlayerXMap = (int) GlobalMap.Biomes[0].Center.x;
        PlayerYMap = (int) GlobalMap.Biomes[0].Center.y;
        SetGlobalEntities();
        SpawnCorridor(DirectionType.North);
        Player.GetComponent<PlayerControls>().ShowHideMenu();
        Player.transform.position = new Vector3(0.0f, 0.0f, Player.transform.position.z);
    }
}
