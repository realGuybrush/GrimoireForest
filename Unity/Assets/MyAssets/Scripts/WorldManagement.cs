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
    public List<GameObject> EntityPrefabs = new List<GameObject>();
    public List<int> GlobalEntities = new List<int>();
    public List<TalkData> GlobalTalks = new List<TalkData>();
    public Map GlobalMap = new Map();
    public GameObject DropPrefab;
    public GameObject TalkCloudPrefab;
    public int PlayerXMap, PlayerYMap;
    int PlayerCurrentXMapOffset = 0;
    GameObject Book;
    DirectionType CameraLookDirection = DirectionType.North;
    //public List<GameObject> CloneItemPrefabs = new List<GameObject>();
    //public List<int> APnum = new List<int>();
    //public List<AnimatorProperties>AP = new List<AnimatorProperties>();
    //public int current_Scene;
    //public Font font;
    //public Passage Pass = null;
    // Use this for initialization
    void Start()
    {
        Book = GameObject.Find("Book");
        Physics2D.IgnoreLayerCollision(0, 0);
        Physics2D.IgnoreLayerCollision(0, 14);
        //Physics2D.IgnoreLayerCollision(0, 11);
        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(12, 11);
        Physics2D.IgnoreLayerCollision(13, 11);
        Physics2D.IgnoreLayerCollision(14, 14);
        Player = GameObject.Find("Player");
        //for (int i = 0; i < ItemPrefabs.Count; i++)
        {
            //ItemPrefabs[i].GetComponent<Item>().Start2();
        }
        SetBiomePrefabs();
        Drop(0, 1, new Vector3(0.0f, 0.0f, 0.0f));
        Drop(3,1,new Vector3(0.0f, 0.0f, 0.0f));
        StopTime();
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
    void Update()
    {
        //if(Pass != null)
        //Enter();
    }
    public void Drop(int itemNumber, int itemCount, Vector3 coordinates)
    {
        for (int i = 0; i < itemCount; i++)
        {
            //GameObject G =
            GameObject.Instantiate(ItemPrefabs[itemNumber], coordinates, new Quaternion(), GameObject.Find("Items").transform);//.GetComponent<Item>().Start2()
            //Environment.transform.GetChild(1).transform.GetChild(Environment.transform.GetChild(1).transform.childCount - 1).gameObject.GetComponent<Item>().Start2();
            //G.GetComponent<Item>().Start2();
        }
    }

    public void CalculatePlayerCorridorOffset(int x)
    {
        int playerWidth = 10;
        PlayerCurrentXMapOffset = -((x + System.Math.Sign(x) * ((int)TileWidth - playerWidth) / 2) / (int)TileWidth) * calculateYDirectionCoeff(CameraLookDirection);
    }
    public void ChangePlayerCoordinates(int x, int y)
    {
        PlayerXMap += x - PlayerCurrentXMapOffset * calculateYDirectionCoeff(CameraLookDirection);
        PlayerYMap += y + PlayerCurrentXMapOffset * calculateXDirectionCoeff(CameraLookDirection);
    }

    public void SetCorrectPlayerPosition(DirectionType walkTo)
    {
        DirectionType enterFrom = (DirectionType)(((int)walkTo + (int)CameraLookDirection + 2) % 4);
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
        CalculatePlayerCorridorOffset((int)Player.transform.position.x);
        UpdateCorridor();
        DeleteCorridor();
        ChangePlayerCoordinates(calculateXDirectionCoeff(enterDirection), calculateYDirectionCoeff(enterDirection));
        SpawnCorridor();
        SetCorrectPlayerPosition(enterDirection);
    }

    int calculateXDirectionCoeff(DirectionType enterDirection)
    {
        return (-((int)enterDirection - 2)) % 2;
    }
    int calculateYDirectionCoeff(DirectionType enterDirection)
    {
        return ((int)enterDirection - 1) % 2;
    }

    public int ItemNumberByName(string itemName)
    {
        for (int i = 0; i < ItemPrefabs.Count; i++)
        {
            if (itemName.Contains(ItemPrefabs[i].name))
            {
                return i;
            }
        }
        return -1;
    }
    public int EntityNumberByName(string entityName)
    {
        for (int i = 0; i < EntityPrefabs.Count; i++)
        {
            if (entityName.Contains(EntityPrefabs[i].name))
            {
                return i;
            }
        }
        return -1;
    }

    public void UpdateCorridor(DirectionType LookTurn = DirectionType.North)
    {
        int xCoeff = calculateXDirectionCoeff(LookTurn);
        int yCoeff = -calculateYDirectionCoeff(LookTurn);
        int offsetTilesLeft = 0;
        int offsetTilesRight = 0;
        Vector2 newCoords;
        Transform chI;
        while (GlobalMap.Tiles[PlayerYMap + offsetTilesLeft* xCoeff][PlayerXMap + offsetTilesLeft* yCoeff].passages[3] == PassageType.Corridor)
        {
            offsetTilesLeft--;
        }
        while (GlobalMap.Tiles[PlayerYMap + offsetTilesLeft * xCoeff][PlayerXMap + offsetTilesRight * yCoeff].passages[1] == PassageType.Corridor)
        {
            offsetTilesRight++;
        }
        for (int i = offsetTilesLeft; i <= offsetTilesRight; i++)
        {
            GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].TileChests = new List<EnvironmentStuffingValues>();
            GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].TileChestsInventry = new List<Inventory>();
            GlobalMap.Tiles[PlayerYMap + i * xCoeff][PlayerXMap + i * yCoeff].TileEntitiesPositions = new List<EntityValues>();
        }
        for (int i = 0; i < Environment.transform.GetChild(1).childCount; i++)
        {
            chI = Environment.transform.GetChild(1).GetChild(i);
            newCoords = calculateMapCoordsForThis(chI.position, LookTurn);
            GlobalMap.Tiles[(int)newCoords.y][(int)newCoords.x].TileItems.Add(new EnvironmentStuffingValues(ShrinkPosition(chI.position), ItemNumberByName(chI.gameObject.name)));
        }
        for (int i = 0; i < Environment.transform.GetChild(2).childCount; i++)
        {
            chI = Environment.transform.GetChild(2).GetChild(i);
            newCoords = calculateMapCoordsForThis(chI.position, LookTurn);
            if (EntityNumberByName(chI.gameObject.name) != -1)
            {
                GlobalMap.Tiles[(int)newCoords.y][(int)newCoords.x].TileEntitiesPositions.Add(new EntityValues(EntityNumberByName(chI.gameObject.name), ShrinkPosition(chI.position), chI.eulerAngles, chI.gameObject.GetComponent<Rigidbody2D>().velocity, chI.gameObject.GetComponent<BasicMovement>().inventory, chI.gameObject.GetComponent<BasicMovement>().thisHealth.values));
            }
        }
        for (int i = 0; i < Environment.transform.GetChild(3).childCount; i++)
        {
            chI = Environment.transform.GetChild(3).GetChild(i);
            newCoords = calculateMapCoordsForThis(chI.position, LookTurn);
            GlobalMap.Tiles[(int)newCoords.y][(int)newCoords.x].TileChests.Add(new EnvironmentStuffingValues(ShrinkPosition(chI.position), chI.gameObject.GetComponent<Chest>().realChest?1:0));
            GlobalMap.Tiles[(int)newCoords.y][(int)newCoords.x].TileChestsInventry.Add(chI.gameObject.GetComponent<Chest>().inventory);
        }
    }

    Vector2 calculateMapCoordsForThis(Vector3 position, DirectionType LookTurn = DirectionType.North)
    {
        int X = PlayerXMap;
        int Y = PlayerYMap;
        X += (int)((position.x + TileWidth / 2) / TileWidth) * calculateYDirectionCoeff(LookTurn);
        Y += (int)((position.x + TileWidth / 2) / TileWidth) * calculateXDirectionCoeff(LookTurn);
        if (X < 0)
            X = 0;
        if (Y < 0)
            Y = 0;
        if (X > GlobalMap.Width - 1)
            X = GlobalMap.Width - 1;
        if (Y > GlobalMap.Height - 1)
            Y = GlobalMap.Height - 1;
        return new Vector2(X, Y);
    }
    Vector3 ShrinkPosition(Vector3 position)
    {
        return new Vector3(position.x % (TileWidth / 2), position.y, position.z);
    }

    public void SaveGame()
    {
        UpdateCorridor();
        Save save = new Save();
        save.ExecuteSaving();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(System.IO.Directory.GetCurrentDirectory() + "/TestSave.sav");//Application.persistentDataPath
        bf.Serialize(file, save);
        file.Close();
    }
    public void LoadGame()
    {
        if (File.Exists(Directory.GetCurrentDirectory() + "/TestSave.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Directory.GetCurrentDirectory() + "/TestSave.sav", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            save.ExecuteLoading();
            SpawnCorridor();
        }
    }
    public List<TalkData> GetTalks()
    {
        return GlobalTalks;
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void UnStopTime()
    {
        Time.timeScale = 1;
    }
}
