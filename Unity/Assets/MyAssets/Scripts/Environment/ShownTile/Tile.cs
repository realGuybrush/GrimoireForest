using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyAssets.Scripts.Environment {

    public class Tile : MonoBehaviour {

        [SerializeField]
        private SerializableDictionaryBase<DirectionType, GameObject> borders, borderPassages;

        [SerializeField]
        private Tilemap ground;

        [SerializeField]
        private Tilemap frontBushes;

        private EnvironmentFactory environmentFactory;

        public void Init(MapTile MT, DirectionType LookTurn = DirectionType.North,
            bool load = false) {
            environmentFactory = EnvironmentFactory.GetInstance;
            //print them all, probably set on first spawn
            //make sky and moon placements as part of Update and put it on movement through various biomes
            //-3 -2 -1 1 R L G
            //wall path
            for (int i = 0; i < 4; i++) {
                if (MT.passages[(i + (int) LookTurn) % 4] == PassageType.Corridor) {
                    borders[(DirectionType) i].SetActive(false);
                    borderPassages[(DirectionType) i].SetActive(false);
                } else if (MT.passages[(i + (int) LookTurn) % 4] == PassageType.Door) {
                    borders[(DirectionType) i].SetActive(false);
                } else {
                    borderPassages[(DirectionType) i].SetActive(false);
                }
            }
            int halfHeight = MT.blocks.Count / 2;
            int halfWidth = MT.GridWidth / 2;
            int currentBlockY;
            for (int y = 0; y < MT.blocks.Count; y++) {
                currentBlockY = y - halfHeight;
                for (int x = 0; x < MT.GridWidth; x++) {
                    ground.SetTile(new Vector3Int(x - halfWidth, currentBlockY, 0),
                        environmentFactory.GetBiomeBlockPrefabs(MT.biome1, MT.biome2)[MT.blocks[y][x]]);
                    frontBushes.SetTile(new Vector3Int(x - halfWidth, currentBlockY, 0),
                        environmentFactory.GetBiomeBlockPrefabs(MT.biome1, MT.biome2)[MT.bushes[y][x]]);
                }
            }

            /*todo: fix stuffing spawning
            for (int i = 0; i < MT.TilePlatforms.Count; i++) {
                Instantiate(environmentFactory.GetBiomePlatformPrefabs(MT.biome1, MT.biome2)[MT.TilePlatforms[i].indexInPrefabs],
                    MT.TilePlatforms[i].location.ToV3(), new Quaternion(), transform);
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
            }*/
        }

        private Vector3 NewBorderPosition(MapTile MT, int x) {
            Vector3 newPosition =
                ground.layoutGrid.GetCellCenterWorld(new Vector3Int(x, MT.GetColumnTopPosition(x), 0));
            newPosition -= new Vector3(ground.cellSize.x * ground.size.x / 2, ground.cellSize.y * ground.size.y / 2, 0);
            return newPosition;
        }

        void SetTileEntities(MapTile MT) {
            //todo: fix this crap; maybe remove this class altogether
            int newAmount;
            //fix create here array of entities in tile to spawn them
            for (int i = 0;
                i < environmentFactory.GetBiomeEntitiesPrefabs(MT.biome1, MT.biome2).Count;
                i++) {
                newAmount = Random.Range(0, environmentFactory.GetBiomeEntitiesAmounts(MT.biome1, MT.biome2)[i]);
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
            return entity;*/ //todo: fix
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
