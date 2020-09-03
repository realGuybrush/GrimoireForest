using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SVector3
{
    public float x;
    public float y;
    public float z;
    public SVector3()
    {
        x = 0.0f;
        y = 0.0f;
        z = 0.0f;
    }
    public SVector3(float X, float Y, float Z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    public SVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public Vector3 ToV3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class Save
{
    //player parameters
    // items
    // quests
    SVector3 playerPosition;
    SVector3 playerRotation;
    SVector3 playerSpeed;
    Characteristics playerCharacteristics;
    Inventory playerInventory;
    Inventory playerMunitions;
    Inventory playerSpells;
    //map
    MapSave MS = new MapSave();
    public void ExecuteSaving()
    {
        GameObject Player = GameObject.Find("Player");
        playerPosition = new SVector3(Player.transform.position);
        playerRotation = new SVector3(Player.transform.eulerAngles);
        playerSpeed = new SVector3(Player.GetComponent<Rigidbody2D>().velocity);
        playerCharacteristics = Player.GetComponent<PlayerControls>().thisHealth.values;
        playerInventory = Player.GetComponent<PlayerControls>().inventory;
        playerMunitions = Player.GetComponent<PlayerControls>().munitions;
        playerSpells = Player.GetComponent<PlayerControls>().spells;
        MS.SaveMap();
    }

    public void ExecuteLoading()
    {
        GameObject.Find("Player").GetComponent<PlayerControls>().LoadData(playerPosition, playerRotation, playerSpeed, playerCharacteristics, playerInventory, playerMunitions, playerSpells);
        MS.LoadMap();
    }
}

[Serializable]
public class MapSave
{
    int PlayerXMap, PlayerYMap;
    Map GlobalMap;
    //List<EntityValues> GlobalEntities = new List<EntityValues>();
    public void SaveMap()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        GlobalMap = WM.GlobalMap;
        PlayerXMap = WM.PlayerXMap;
        PlayerYMap = WM.PlayerYMap;
        //for (int i = 0; i < GlobalMap.GlobalEntities.Count; i++)
        //{
        //    GlobalEntities.Add(new EntityValues(WM.EntityNumberByName(GlobalMap.GlobalEntities[i].name), GlobalMap.GlobalEntities[i].transform.position, GlobalMap.GlobalEntities[i].transform.eulerAngles, GlobalMap.GlobalEntities[i].GetComponent<Rigidbody2D>().velocity, GlobalMap.GlobalEntities[i].GetComponent<BasicMovement>().inventory, GlobalMap.GlobalEntities[i].GetComponent<BasicMovement>().thisHealth.values));
        //}
    }
    public void LoadMap()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        GameObject.Find("WorldManager").GetComponent<WorldManagement>().GlobalMap = GlobalMap;
        WM.PlayerXMap = PlayerXMap;
        WM.PlayerYMap = PlayerYMap;
        //for (int i = 0; i < GlobalEntities.Count; i++)
        //{
        //    WM.GlobalMap.GlobalEntities.Add(WM.SpawnEntity(GlobalEntities[i]));
        //}
        //GameObject.Find("Entities").transform.DetachChildren();
    }
}
