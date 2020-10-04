using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManagement : MonoBehaviour
{
    public List<List<BiomeTilesData>> BiomePrefabs = new List<List<BiomeTilesData>>();

    public float TileWidth = 80.5f;
    public void SetBiomePrefabs()//set as constructor, if it will be separate class
    {
        for (int i = 0; i < BiomeTilesData.BiomesAmount; i++)
        {
            BiomePrefabs.Add(new List<BiomeTilesData>());
            for (int j = 0; j < BiomeTilesData.BiomesAmount; j++)
            {
                BiomePrefabs[i].Add(new BiomeTilesData());
            }
        }
        //(GameObject)Resources.Load("Prefabs\\Background and objects\\Default\\background");
        BiomePrefabs[0][0].aggressive = false;
        BiomePrefabs[0][0].immortal = false;
        BiomePrefabs[0][0].Size = MaxBiomeSizeInPercents.Enormous;
        BiomePrefabs[0][0].Sky = (Sprite)Resources.Load("Pictures\\Environment\\Forest\\stars.png");
        BiomePrefabs[0][0].Moon = (Sprite)Resources.Load("Pictures\\Environment\\Forest\\moon.png");
        BiomePrefabs[0][0].TilePrefab = (GameObject)Resources.Load("Prefabs\\Environment\\Forest\\ForestTile");
        BiomePrefabs[0][0].PlatformPrefab.Add((GameObject)Resources.Load("Prefabs\\Environment\\Forest\\ForestPlatform"));
        BiomePrefabs[0][0].PlatformPrefab.Add((GameObject)Resources.Load("Prefabs\\Environment\\Forest\\BigTree"));
        BiomePrefabs[0][0].PlatformPrefab.Add((GameObject)Resources.Load("Prefabs\\Environment\\Forest\\TreeBranch"));
        BiomePrefabs[0][0].PlatformPrefab = RemoveAllNull(BiomePrefabs[0][0].PlatformPrefab);
        BiomePrefabs[0][0].EntitiesPrefabs.Add(1);
        BiomePrefabs[0][0].EntitiesAmounts.Add(2);
        //public List<GameObject> CoverPrefabs;
        //public List<GameObject> EntitiesPrefabs;

        //fix add somewhere predefinition of tile monster spawn pattern
    }

    public List<GameObject> RemoveAllNull(List<GameObject> L)
    {
        for (int i = 0; i < L.Count; i++)
        {
            if (L[i] == null)
            {
                L.RemoveAt(i);
                i--;
            }
        }
        return L;
    }
    public void DeleteCorridor()
    {
        for(int j = 0; j<4; j++)
        for (int i = Environment.transform.GetChild(j).transform.childCount - 1; i > 1; i--)
        {
            GameObject.Destroy(Environment.transform.GetChild(j).transform.GetChild(i).gameObject);
        }
        //for (int i = Environment.transform.GetChild(1).transform.childCount - 1; i > 1; i--)
        //{
        //    GameObject.Destroy(Environment.transform.GetChild(1).transform.GetChild(i).gameObject);
        //}
        for (int i = Environment.transform.childCount-1; i > 3; i--)
        {
            GameObject.Destroy(Environment.transform.GetChild(i).gameObject);
        }
        Player.GetComponent<PlayerControls>().RemoveDoors();
    }
    public void SpawnCorridor(MapTile MT, DirectionType LookTurn = DirectionType.North)
    {
        int offsetTilesLeft = 0;
        int offsetTilesRight = 0;
        while (GlobalMap.Tiles[PlayerYMap][PlayerXMap + offsetTilesLeft].passages[3] == PassageType.Corridor)
        {
            offsetTilesLeft--;
        }
        while (GlobalMap.Tiles[PlayerYMap][PlayerXMap + offsetTilesRight].passages[1] == PassageType.Corridor)
        {
            offsetTilesRight++;
        }
        for (int i = offsetTilesLeft; i <= offsetTilesRight; i++)
        {
            SpawnTile(i, GlobalMap.Tiles[PlayerYMap+i*(-(int)LookTurn-2)%2][PlayerXMap+i*(-(int)LookTurn-1)%2], LookTurn);
        }
        GameObject.Find("Main Camera").GetComponent<Camera_Movement>().SetCameraBoundaries(offsetTilesLeft, offsetTilesRight);
    }

    public void SpawnTile(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North)
    {
        GameObject GO = GameObject.Instantiate(BiomePrefabs[(int)MT.biome1][(int)MT.biome2].TilePrefab, new Vector3(xTileOffset * TileWidth, 0.0f, 0.0f), new Quaternion(), Environment.transform);
        //GameObject GO2 = GameObject.Instantiate(BiomePrefabs[(int)type1][(int)type2].PlatformPrefab);//print them all, probably set on first spawn
        //make sky and moon placemens as part of Update and put it on movement through various biomes
        //-3 -2 -1 1 R L G
        //wall path
        GO.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        if (MT.passages[(0 + (int)LookTurn) % 4] == PassageType.Door)
        {
            GO.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
            GO.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            GO.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
            GO.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        }

        if ((MT.passages[(1 + (int)LookTurn) % 4] != PassageType.No) && (MT.passages[(1 + (int)LookTurn) % 4] != PassageType.SecretDoor))
        {
            GO.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
        }
        if (MT.passages[(1 + (int)LookTurn) % 4] != PassageType.Door)
        {
            GO.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
        }

        if (MT.passages[(2 + (int)LookTurn) % 4] == PassageType.Door)
        {
            GO.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            GO.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
        }

        if ((MT.passages[(3 + (int)LookTurn) % 4] != PassageType.No) && (MT.passages[(3 + (int)LookTurn) % 4] != PassageType.SecretDoor))
        {
            GO.transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
        }
        if (MT.passages[(3 + (int)LookTurn) % 4] != PassageType.Door)
        {
            GO.transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < MT.TilePlatforms.Count; i++)
        {
            Instantiate(BiomePrefabs[(int)MT.biome1][(int)MT.biome2], MT.TilePlatforms[i], Environment.transform.GetChild(0).transform);
        }
        for (int i = 0; i < MT.TileChests.Count; i++)
        {
            SpawnChest(MT, i);
        }
        for (int i = 0; i < MT.TileItems.Count; i++)
        {
            Drop(MT.TileItems[i].indexInPrefabs, 1, MT.TileItems[i].location.ToV3());
        }
        if (MT.TileEntitiesPositions.Count == 0)
        {
            SetTileEntities(MT);
        }
        for (int i = 0; i < MT.TileEntitiesPositions.Count; i++)
        {
            SpawnEntity(MT.TileEntitiesPositions[i], i);
        }
    }
    void SetTileEntities(MapTile MT)
    {
        int newAmount;
        //fix create here array of entities in tile to spawn them
        for (int i = 0; i < BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs.Count; i++)
        {
            newAmount = Random.Range(0, BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesAmounts[i]);
            for (int j = 0; j < newAmount; j++)
            {
                MT.TileEntitiesPositions.Add(new EntityValues(BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i],
                                                              new Vector3(Random.Range((float)(-TileWidth / 2), (float)(TileWidth / 2)), 0.0f, 0.0f),
                                                              new Vector3(0.0f, 0.0f, 0.0f),
                                                              new Vector3(0.0f, 0.0f, 0.0f),
                                                              new Inventory(),//EntityPrefabs[BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<BasicMovement>().inventory
                                                              new Characteristics()));//EntityPrefabs[BiomePrefabs[(int)MT.biome1][(int)MT.biome2].EntitiesPrefabs[i]].GetComponent<BasicMovement>().thisHealth.values
            }
        }
    }

    void Instantiate(BiomeTilesData BTD, EnvironmentStuffingValues ESV, Transform parent)
    {
        GameObject.Instantiate(BTD.PlatformPrefab[ESV.indexInPrefabs], ESV.location.ToV3(), new Quaternion(), parent);
    }

    public GameObject SpawnEntity(EntityValues EV, int parentIndex = -1)
    {
        GameObject entity = GameObject.Instantiate(EntityPrefabs[EV.indexInPrefabs], GameObject.Find("Entities").transform);
        entity.transform.position = EV.location.ToV3();
        entity.transform.eulerAngles = EV.rotation.ToV3();
        entity.GetComponent<Rigidbody2D>().velocity = EV.velocity.ToV3();
        entity.GetComponent<BasicMovement>().inventory = EV.inventory;
        entity.GetComponent<BasicMovement>().thisHealth.values = EV.characteristics;
        if (entity.transform.GetChild(entity.transform.childCount - 1).name == "Attached")
        {
            for (int i = 0; i < entity.transform.GetChild(entity.transform.childCount - 1).childCount; i++)
            {
                entity.transform.GetChild(entity.transform.childCount - 1).GetChild(i).GetComponent<EnemyMovement>().attachedTo = parentIndex;
            }
        }
        return entity;
    }

    public void SpawnChest(MapTile MT, int i)
    {
        GameObject temp;
        if (MT.TileChests[i].indexInPrefabs == 0)
        {
            temp = GameObject.Instantiate(DropPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(), Environment.transform.GetChild(3).transform);
            temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
        }
        else
        {
            temp = GameObject.Instantiate(BiomePrefabs[(int)MT.biome1][(int)MT.biome2].ChestPrefab, MT.TileChests[i].location.ToV3(), new Quaternion(), Environment.transform.GetChild(3).transform);
            temp.GetComponent<Chest>().inventory = MT.TileChestsInventry[i];
        }
    }

    public void MapGeneration()
    {
        GlobalMap.Generate_Map(BiomePrefabs[0]);
        PlayerXMap = (int)GlobalMap.Biomes[0].Center.x;
        PlayerYMap = (int)GlobalMap.Biomes[0].Center.y;
        SpawnCorridor(GlobalMap.Tiles[PlayerXMap][PlayerYMap], DirectionType.North);
        Player.GetComponent<PlayerControls>().ShowHideMenu();
        Player.transform.position = new Vector3(0.0f, 0.0f, Player.transform.position.z);
    }
}
