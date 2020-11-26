using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Events
{
    public bool PLAYING = false;
    public string EvType;
    public bool activatable = true;
    public bool turn_off_after_activation = false;
    public char parameter_to_activate = ' ';
    public int index_of_activatable_parameter1 = 0;//in case of talking, this is number of activated line
    public int index_of_activatable_parameter2 = 0;
    public int index_of_activatable_parameter3 = 0;
    public string stringParameter = "";
    public float wait_for_in_seconds = 0.0f;
    public Vector3 coords = new Vector3();


    public Events(string t, bool a, bool o, char p, int j, int k, int i, string s, float f, Vector3 v)
    {
        EvType = t;
        activatable = a;
        turn_off_after_activation = o;
        parameter_to_activate = p;
        index_of_activatable_parameter1 = j;
        index_of_activatable_parameter2 = k;
        index_of_activatable_parameter3 = i;
        stringParameter = s;
        wait_for_in_seconds = f;
        coords = v;
    }

    public void DoEvent()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        if (turn_off_after_activation)
            activatable = false;
        switch (EvType)
        {
            case "spawn":
                //spawn anything such as vine or explosion
                GameObject bullet = GameObject.Instantiate(GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[index_of_activatable_parameter1], coords, Quaternion.identity,GameObject.Find("Projectiles").transform);
                bullet.GetComponent<Projectile>().ignore = GameObject.Find(stringParameter);
                bullet.transform.eulerAngles = new Vector3(0.0f, 0.0f, (float)index_of_activatable_parameter2);
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * (float)index_of_activatable_parameter3;
                break;
            case "talk":
                //activate talk line
                break;
            case "replace":
                //special, replace this item with another item in inventory; make sure it neither replace all such items, nor cause infinite loop
                break;
            case "add":
                //instantly add another item to inventory
                if (WM.ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>().itemValues.type != "Spell")
                {
                    GameObject.Find("Player").GetComponent<PlayerControls>().inventory.Add1(WM.ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>());
                }
                else
                {
                    if (!GameObject.Find("Player").GetComponent<PlayerControls>().spells.Items.Contains(index_of_activatable_parameter1))
                    GameObject.Find("Player").GetComponent<PlayerControls>().spells.Add1(WM.ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>());
                }
                break;
            case "addonlyone":
                //instantly add another item to inventory
                if (WM.ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>().itemValues.type != "Spell")
                {
                    if (!GameObject.Find("Player").GetComponent<PlayerControls>().inventory.Items.Contains(index_of_activatable_parameter1))
                        GameObject.Find("Player").GetComponent<PlayerControls>().inventory.Add1(GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>());
                }
                else
                {
                    if (!GameObject.Find("Player").GetComponent<PlayerControls>().spells.Items.Contains(index_of_activatable_parameter1))
                        GameObject.Find("Player").GetComponent<PlayerControls>().spells.Add1(GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[index_of_activatable_parameter1].GetComponent<Item>());
                }
                break;
            default:
                break;
        }
    }

    public void WTFWait()
    {
        //yield return new WaitForSeconds(wait_for_in_seconds);
        int fcukingtonofticks = 0;
        while ((float)fcukingtonofticks < 1000000 * wait_for_in_seconds)
        {
            fcukingtonofticks++;
        }
    }
}
