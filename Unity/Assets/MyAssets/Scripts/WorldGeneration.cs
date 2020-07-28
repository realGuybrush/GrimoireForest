using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManagement : MonoBehaviour
{
    public List<List<BiomeTilesData>> BiomePrefabs = new List<List<BiomeTilesData>>();

    public int TileWidth = 300;
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
        BiomePrefabs[0][0].PlatformPrefab = (GameObject)Resources.Load("Prefabs\\Environment\\Forest\\ForestPlatform");
        //public List<GameObject> CoverPrefabs;
        //public List<GameObject> EntitiesPrefabs;
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
    }

    public void SpawnTile(int xTileOffset, MapTile MT, DirectionType LookTurn = DirectionType.North)
    {
        GameObject GO = GameObject.Instantiate(BiomePrefabs[(int)MT.biome1][(int)MT.biome2].TilePrefab, new Vector3(xTileOffset*TileWidth, 0.0f, 0.0f), new Quaternion(), Environment.transform);
        //GameObject GO2 = GameObject.Instantiate(BiomePrefabs[(int)type1][(int)type2].PlatformPrefab);//print them all, probably set on first spawn
        //make sky and moon placemens as part of Update and put it on movement through various biomes
        //-3 -2 -1 1 R L G
        //wall path
        GO.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        GO.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<BackGroundMovement>().tileOffset = xTileOffset;
        if (MT.passages[(0+(int)LookTurn)%4] == PassageType.Door)
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
    }

    public void MapGeneration()
    {
        GlobalMap.Generate_Map(BiomePrefabs[0]);
    }
}
