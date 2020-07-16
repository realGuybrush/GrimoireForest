using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsDepiction : MonoBehaviour
{
    PlayerControls Player;
    WorldManagement WM;
    public List<GameObject> ArmsAndSpells = new List<GameObject>();
    public GameObject FloatingTile1;
    public GameObject FloatingTile2;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Player2 = GameObject.Find("Player");
        if (Player2 != null)
        {
            Player = Player2.GetComponent<PlayerControls>();
        }
        WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateWSNumber()
    {
        FloatingTile1.transform.position = ArmsAndSpells[Player.weaponSlotNumber].transform.position;
        //FloatingTile2.transform.position = ArmsAndSpells[Player.spellSlotNumber + 5].transform.position;
    }
    public void Update2(int index)
    {
        if (index < 5)
        {
            if (Player.munitions.Items[index + 6] > -1)
            {
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().sprite = WM.ItemPrefabs[Player.munitions.Items[index + 6]].GetComponent<Item>().InventoryImage;
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            }
            else
            {
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().sprite = null;
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
        }
        else
        {
            if (Player.munitions.Items[index - 5] > -1)
            {
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().sprite = WM.ItemPrefabs[Player.munitions.Items[index - 5]].GetComponent<Item>().InventoryImage;
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            }
            else
            {
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().sprite = null;
                ArmsAndSpells[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
        }
    }
}
