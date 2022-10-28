using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class Tile : MonoBehaviour {
        private EnvironmentFactory environmentFactory;
        public void Init(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North,
            bool load = false) {
            environmentFactory = EnvironmentFactory.GetInstance;
            //GameObject GO2 = GameObject.Instantiate(BiomePrefabs[(int)type1][(int)type2].PlatformPrefab);//print them all, probably set on first spawn
            //make sky and moon placemens as part of Update and put it on movement through various biomes
            //-3 -2 -1 1 R L G
            //wall path
            transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset =
                xTileOffset;
            transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject
                .GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
            transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset =
                xTileOffset;
            transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset =
                xTileOffset;
            if (MT.passages[(0 + (int) LookTurn) % 4] == PassageType.Door) {
                transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
            } else {
                transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
            }

            if ((MT.passages[(1 + (int) LookTurn) % 4] != PassageType.No) &&
                (MT.passages[(1 + (int) LookTurn) % 4] != PassageType.SecretDoor)) {
                transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
            }
            if (MT.passages[(1 + (int) LookTurn) % 4] != PassageType.Door) {
                transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
            }

            if (MT.passages[(2 + (int) LookTurn) % 4] == PassageType.Door) {
                transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
            } else {
                transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
            }

            if ((MT.passages[(3 + (int) LookTurn) % 4] != PassageType.No) &&
                (MT.passages[(3 + (int) LookTurn) % 4] != PassageType.SecretDoor)) {
                transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
            }
            if (MT.passages[(3 + (int) LookTurn) % 4] != PassageType.Door) {
                transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(false);
            }

            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < MT.BlocksInTile; j++) {
                    if (MT.blocks[i][j] != (int) BlockType.Empty)
                        Instantiate(
                            environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2]
                                .BlockPrefabs[MT.blocks[i][j]],
                            new Vector3(
                                (j - MT.BlocksInTile / 2) * 1.5f + xTileOffset * (MT.BlocksInTile * 1.5f),
                                (i) * -1.5f, 0.0f), new Quaternion(), transform.GetChild(6));
                }
            }

            for (int i = 0; i < MT.TilePlatforms.Count; i++) {
                Instantiate(environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2], MT.TilePlatforms[i],
                    transform);
            }
            for (int i = 0; i < MT.TileChests.Count; i++) {
                SpawnChest(MT, i);
            }
            for (int i = 0; i < MT.TileItems.Count; i++) {
                //Drop(MT.TileItems[i].indexInPrefabs, 1, MT.TileItems[i].location.ToV3());
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
            for (int i = 0;
                i < environmentFactory.BiomeDTOs[(int) MT.biome1][(int) MT.biome2].EntitiesPrefabs.Count;
                i++) {
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
            /*GameObject entity =
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
            return entity;*///todo: fix
            return null;
        }

        public void SpawnChest(MapTile MT, int i) {
            //GameObject temp;
            if (MT.TileChests[i].indexInPrefabs == 0) {
                //temp = GameObject.Instantiate(DropPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(),
                //    Environment.transform.GetChild(3).transform);
                //temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
            } else {
                //todo: fix this crap
                //temp = GameObject.Instantiate(environmentFactory.BiomePrefabs[(int)MT.biome1][(int)MT.biome2].ChestPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(), Environment.transform.GetChild(3).transform);
                //temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
            }
        }
    }

}
