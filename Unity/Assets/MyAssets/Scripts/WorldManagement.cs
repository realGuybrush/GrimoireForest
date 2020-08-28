using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*public class AnimatorProperties
{
	public string state_name = "";
	public List<string>BoolsNames = new List<string>();
	public List<bool>Bools = new List<bool>();
	public List<string>FloatsNames = new List<string>();
	public List<float>Floats = new List<float>();
	//public List<bool>Bools = new List<bool>();
	//public List<bool>Bools = new List<bool>();
}*/

public partial class WorldManagement : MonoBehaviour 
{
    //public string initial_scene_data;
    //public List<Scene> Scenes = new List<Scene>();
    public GameObject Player;
    public GameObject Environment;
	public List<GameObject> ItemPrefabs = new List<GameObject>();
    public Map GlobalMap = new Map();
    public GameObject DropPrefab;
    int PlayerXMap, PlayerYMap;
    int PlayerCurrentXMapOffset = 0;
    DirectionType CameraLookDirection = DirectionType.North;
	//public List<GameObject> CloneItemPrefabs = new List<GameObject>();
	//public List<int> APnum = new List<int>();
	//public List<AnimatorProperties>AP = new List<AnimatorProperties>();
	//public int current_Scene;
	//public Font font;
	//public Passage Pass = null;
	// Use this for initialization
	void Start () 
	{
        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(13, 11);
        Player = GameObject.Find("Player");
        for (int i = 0; i < ItemPrefabs.Count; i++)
        {
            ItemPrefabs[i].GetComponent<Item>().Start2();
        }
        SetBiomePrefabs();
        //MapGeneration();
        //PlayerXMap = (int)GlobalMap.Biomes[0].Center.x;
        //PlayerYMap = (int)GlobalMap.Biomes[0].Center.y;
        //SpawnCorridor(GlobalMap.Tiles[PlayerXMap][PlayerYMap]);
        //current_Scene = 0;//move it in load after it is fixed
        //LoadInitialScenesData();
        //SaveItemsData();
        //GameObject.Find("Player").GetComponent<Inventory>().takeall = true;
        //LoadWHUD();
    }
	
	// Update is called once per frame
	void Update () 
	{
		//if(Pass != null)
			//Enter();
	}

    public void Drop(int itemNumber, int itemCount, Vector3 coordinates)
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject.Instantiate(ItemPrefabs[itemNumber], coordinates, new Quaternion());
        }
    }

    public void CalculatePlayerCorridorOffset(int x)
    {
        int playerWidth = 10;
        PlayerCurrentXMapOffset = -((x + System.Math.Sign(x)*(TileWidth+playerWidth) / 2) / TileWidth) * calculateXDirectionCoeff(CameraLookDirection);
    }
    public void ChangePlayerCoordinates(int x, int y)
    {
        PlayerXMap += -x + PlayerCurrentXMapOffset * calculateXDirectionCoeff(CameraLookDirection);
        PlayerYMap += PlayerCurrentXMapOffset * calculateYDirectionCoeff(CameraLookDirection) + -y * calculateXDirectionCoeff(CameraLookDirection);
    }

    public void SetCorrectPlayerPosition(DirectionType walkTo)
    {
        DirectionType enterFrom = (DirectionType)(((int)walkTo + (int) CameraLookDirection + 2) % 4);
        if ((enterFrom == DirectionType.North) || (enterFrom == DirectionType.South))
        {
            Player.transform.position = new Vector3(0.0f, Player.transform.position.y, 0.0f);
        }
        else
        {
            if (enterFrom == DirectionType.East)
            {
                Player.transform.position = new Vector3(TileWidth / 2, Player.transform.position.y, 0.0f);
            }
            else
            {
                Player.transform.position = new Vector3(-TileWidth / 2, Player.transform.position.y, 0.0f);
            }
        }
    }

    public void EnterDoor(DirectionType enterDirection)
    {
        DeleteCorridor();
        ChangePlayerCoordinates(calculateYDirectionCoeff(enterDirection), calculateXDirectionCoeff(enterDirection));
        SpawnCorridor(GlobalMap.Tiles[PlayerXMap][PlayerYMap]);
        SetCorrectPlayerPosition(enterDirection);
    }

    int calculateXDirectionCoeff(DirectionType enterDirection)
    {
        return ((int)enterDirection - 1) % 2;
    }
    int calculateYDirectionCoeff(DirectionType enterDirection)
    {
        return ((int)enterDirection - 2) % 2;
    }
}
