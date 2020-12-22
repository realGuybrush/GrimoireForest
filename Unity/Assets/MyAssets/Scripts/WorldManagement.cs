using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public partial class WorldManagement : MonoBehaviour
{
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
    // Use this for initialization
    void Start()
    {
        EnableUI();
        Physics2D.IgnoreLayerCollision(0, 0);
        Physics2D.IgnoreLayerCollision(0, 11);
        Physics2D.IgnoreLayerCollision(0, 14);
        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(12, 11);
        Physics2D.IgnoreLayerCollision(13, 11);
        Physics2D.IgnoreLayerCollision(14, 14);
        Player = GameObject.Find("Player");
        SetBiomePrefabs();
        Drop(0, 1, new Vector3(0.0f, 0.0f, 0.0f));
        Drop(3,1,new Vector3(0.0f, 0.0f, 0.0f));
        StopTime();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableUI()
    {
        Book = GameObject.Find("Book");
        Book.GetComponent<MenusTrigger>().InitAll();
        Book.GetComponent<MenusTrigger>().ShowInv(false);
        Book.GetComponent<MenusTrigger>().ShowSpell(false);
        Book.GetComponent<MenusTrigger>().ShowTrade(false);
    }

    public void Drop(int itemNumber, int itemCount, Vector3 coordinates)
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject.Instantiate(ItemPrefabs[itemNumber], coordinates, new Quaternion(), GameObject.Find("Items").transform);//.GetComponent<Item>().Start2()
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
        GameObject.Find("Player").GetComponent<PlayerControls>().Res();
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
            DeleteCorridor();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Directory.GetCurrentDirectory() + "/TestSave.sav", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            save.ExecuteLoading();
            SpawnCorridor(DirectionType.North, true);
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
    void AdjustBGPartPositions(int x, int y, int tileIndexAsChildOfEnv, int BGPartNumberAsChild, int xOffset = 0)
    {
        float inclinedOffsety = 0.0f;
        int j = 0;
        int k = 0;
        int active = 0;
        if (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(0).gameObject.activeSelf)
            active = 0;
        if (Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(1).gameObject.activeSelf)
            active = 1;
        for (int i = 0; i < Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).childCount; i++)//need children of active child of BGPartNumberAsChild
        {
            j = 0;
            k = ((int)(Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).transform.position.x / 1.5f) + GlobalMap.Tiles[y][x].blocks[0].Count / 2);
            if (k < 0)
                k = 0;
            if (k > GlobalMap.Tiles[y][x].blocks[0].Count - 1)
                k = GlobalMap.Tiles[y][x].blocks[0].Count - 1;
            for (j = 0; j < 9; j++)
            {
                if (GlobalMap.Tiles[y][x].blocks[j][k] != 4)
                    break;
            }
            if ((GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.InclLeft)|| (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncLeftBushD)||
                (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncLeftBushU) || (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncLeftBushUD)||
                (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.InclRight) || (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncRightBushD) ||
                (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncRightBushU) || (GlobalMap.Tiles[y][x].blocks[j][k] == (int)BlockType.IncRightBushUD))
            {
                inclinedOffsety = 0.5f;
            }
            Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position = new Vector3(Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.x,
                                                             Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.y + (4 - j - inclinedOffsety) * 1.5f,
                                                             Environment.transform.GetChild(tileIndexAsChildOfEnv).GetChild(BGPartNumberAsChild).GetChild(active).GetChild(i).position.z + 0.0f);
        }
    }
}
