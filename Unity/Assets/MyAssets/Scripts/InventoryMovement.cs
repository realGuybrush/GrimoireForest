using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InventoryMovement : MonoBehaviour
{
    Inventory playerInventory;
    Inventory otherInventory;
    List<GameObject> WeaponList;
    public int width = 3;
    public int height = 6;
    public float gridWidth = 150;
    public float gridHeight = 240;
    public float xOffset = 0.0f;
    List<Vector3> PrimaryMenuLocations = new List<Vector3>();
    List<GameObject> itemDepiction = new List<GameObject>();
    int clicked = -1;
    public GameObject itemInInvPrefab;
    float MenuOffsetX;
    float MenuOffsetY;
    Vector3 clickedPrevLocation;

    public List<GameObject> SecondaryMenuItems = new List<GameObject>();
    List<Vector3> SecondaryMenuLocations = new List<Vector3>();
    bool primary = true;
    bool prevprimary = true;

    private void Start()
    {
        MenuOffsetX = GameObject.Find("Menus").transform.position.x;
        MenuOffsetY = GameObject.Find("Menus").transform.position.y;
        for (int i = 0; i < SecondaryMenuItems.Count; i++)
        {
            SecondaryMenuLocations.Add(SecondaryMenuItems[i].transform.position);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Clicked(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (clicked > -1)
        {
            if (itemDepiction.Count > clicked)
            {
                if (primary)
                {
                    itemDepiction[clicked].transform.position = Input.mousePosition;
                }
                else
                {
                    SecondaryMenuItems[clicked].transform.position = Input.mousePosition;
                }
            }
        }
    }

    public void ShowHide()
    {
        if (clicked >= 0)
        {
            if (primary)
            {
                itemDepiction[clicked].transform.position = clickedPrevLocation;
            }
            else
            {
                SecondaryMenuItems[clicked].transform.position = clickedPrevLocation;
            }
            clicked = -1;
        }
        UploadToHUD();
    }

    public void SetInv(Inventory PI, Inventory OI)//, List<GameObject> WL
    {
        playerInventory = PI;
        otherInventory = OI;
        //WeaponList = WL;
        CalculateItemPositions();
    }

    private void CalculateItemPositions()
    {
        if (width == 0 || height == 0)
        {
            return;
        }

        float posX;
        float posY;
        float buttonWidth;
        float buttonHeight;
        posX = -gridWidth / 2 + (gridWidth / width) / 2;
        posY = gridHeight / 2 - (gridHeight / height) / 2;
        buttonWidth = gridWidth / width;
        buttonHeight = gridHeight / height;
        PrimaryMenuLocations = new List<Vector3>();
        for (int i = 0; i < itemDepiction.Count; i++)
        {
            GameObject.Destroy(itemDepiction[i]);
        }
        itemDepiction = new List<GameObject>();
        int I = (playerInventory.maxAmount > (width * height)) ? (width * height) : playerInventory.maxAmount;
        for (int i = 0; i < I; i++)
        {
            PrimaryMenuLocations.Add(new Vector3(posX + buttonWidth * (i % width), posY - buttonHeight * (int)(i / width), 0.0f));
            itemDepiction.Add(Instantiate(itemInInvPrefab, transform));
            itemDepiction[i].transform.position = transform.position + new Vector3(PrimaryMenuLocations[i].x + xOffset, PrimaryMenuLocations[i].y, 0.0f);
        }
    }
    public void UploadToHUD()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        if (playerInventory != null)
        {
            for (int i = 0; i < PrimaryMenuLocations.Count; i++)
            {
                itemDepiction[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
            }
            int I = (playerInventory.maxAmount > PrimaryMenuLocations.Count) ? PrimaryMenuLocations.Count : playerInventory.maxAmount;
            for (int i = 0; i < I; i++)
            {
                if (playerInventory.Items.Count > i)
                {
                    if (playerInventory.Items[i] >= 0)
                    {
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().sprite = WM.ItemPrefabs[playerInventory.Items[i]].GetComponent<Item>().InventoryImage;//.GetComponent<SpriteRenderer>().sprite;
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
                        /*itemDepiction[i].GetComponent<UnityEngine.UI.Image>().SetNativeSize();
                        Rect R = itemDepiction[i].GetComponent<UnityEngine.UI.Image>().rectTransform.rect;
                        float shrinkCoeff = (R.width < R.height) ? (R.height / 30) : (R.width / 30);
                        R.width /= shrinkCoeff;
                        R.height /= shrinkCoeff; 
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta = new Vector2(R.width, R.height);//shrink it
                        //and that's how we did shrinking in my days*/
                    }
                }
            }
        }
        if (otherInventory != null)
        {
            for (int i = 0; i < SecondaryMenuItems.Count; i++)
            {
                SecondaryMenuItems[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
            }
            for (int i = 0; i < SecondaryMenuItems.Count; i++)
            {
                if (otherInventory.Items.Count > i)
                {
                    if (otherInventory.Items[i] >= 0)
                    {
                        SecondaryMenuItems[i].GetComponent<UnityEngine.UI.Image>().sprite = WM.ItemPrefabs[otherInventory.Items[i]].GetComponent<Item>().InventoryImage;
                        SecondaryMenuItems[i].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
                    }
                }
            }
        }
    }
    private int CalculateButtonNumberByCoordinates(float X, float Y)
    {
        prevprimary = primary;
        if ((X - xOffset >= (-gridWidth / 2)) && (X - xOffset <= (gridWidth / 2)) && (Y >= (-gridHeight / 2)) && (Y <= (gridHeight / 2)))
        {
            primary = true;
            return ((int)((X - xOffset + (gridWidth / 2)) / (gridWidth / width)) - ((int)((Y - (gridHeight / 2)) / (gridHeight / height))) * width);
        }
        else
        {
            for (int i = 0; i < SecondaryMenuItems.Count; i++)
            {
                if ((X >= (SecondaryMenuLocations[i].x - MenuOffsetX - 15)) &&
                    (X <= (SecondaryMenuLocations[i].x - MenuOffsetX + 15)) &&
                    (Y >= (SecondaryMenuLocations[i].y - MenuOffsetY - 15)) &&
                    (Y <= (SecondaryMenuLocations[i].y - MenuOffsetY + 15)))
                {
                    primary = false;
                    return i;
                }
            }
        }
        return -1;
    }

    private bool ItemSwap(int index1, int index2, Inventory inventory1, Inventory inventory2, List<GameObject> buttons1, List<GameObject> buttons2, List<Vector3> locations, bool onlyOneInDestination = false)
    {
        int maxStack;
        if (onlyOneInDestination)
        {
            maxStack = 1;
        }
        else
        {
            Item I = GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[inventory1.Items[index1]].GetComponent<Item>();
            I.Start2();
            maxStack = I.itemValues.maxStack;
        }
        if (inventory1.Items[index1] != inventory2.Items[index2])
        {
            int x = inventory1.Items[index1];
            inventory1.Items[index1] = inventory2.Items[index2];
            inventory2.Items[index2] = x;
            x = inventory1.stacks[index1];
            inventory1.stacks[index1] = inventory2.stacks[index2];
            inventory2.stacks[index2] = x;
            Sprite x2 = buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite;
            buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite = buttons2[index2].GetComponent<UnityEngine.UI.Image>().sprite;
            buttons2[index2].GetComponent<UnityEngine.UI.Image>().sprite = x2;
            Color x3 = buttons1[index1].GetComponent<UnityEngine.UI.Image>().color;
            buttons1[index1].GetComponent<UnityEngine.UI.Image>().color = buttons2[index2].GetComponent<UnityEngine.UI.Image>().color;
            buttons2[index2].GetComponent<UnityEngine.UI.Image>().color = x3;
            buttons1[index1].transform.position = locations[index1];
            return true;
        }
        else
        {
            if (maxStack >= (inventory1.stacks[index1] + inventory2.stacks[index2]))
            {
                inventory2.Items[index2] = inventory1.Items[index1];
                inventory1.Items[index1] = -1;
                inventory2.stacks[index2] += inventory1.stacks[index1];
                inventory1.stacks[index1] = 0;
                buttons2[index2].GetComponent<UnityEngine.UI.Image>().sprite = buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite;
                buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite = null;
                buttons2[index2].GetComponent<UnityEngine.UI.Image>().color = buttons1[index1].GetComponent<UnityEngine.UI.Image>().color;
                buttons1[index1].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); ;
                buttons1[index1].transform.position = locations[index1];
                return true;
            }
            else
            {
                int x = maxStack - inventory2.stacks[index2];
                inventory2.stacks[index2] += maxStack - inventory1.stacks[index1];
                inventory1.stacks[index1] -= x;
                return false;
            }
        }
    }
    public void Clicked(float X, float Y)
    {
        int clicked2 = CalculateButtonNumberByCoordinates(X - MenuOffsetX, Y - MenuOffsetY);
        if (primary)
        {
            if ((clicked == -1) && (clicked2 != -1))
            {
                if (playerInventory.Items[clicked2] != -1)
                {
                    clicked = clicked2;
                    clickedPrevLocation = itemDepiction[clicked].transform.position;
                }
            }
            else
            {
                if (clicked2 > -1)
                {
                    if (primary == prevprimary)
                    {
                        if(ItemSwap(clicked, clicked2, playerInventory, playerInventory, itemDepiction, itemDepiction, PrimaryMenuLocations))
                        clicked = -1;
                    }
                    else
                    {
                        if(ItemSwap(clicked, clicked2, otherInventory, playerInventory, SecondaryMenuItems, itemDepiction, SecondaryMenuLocations))
                        clicked = -1;
                    }
                }
                else
                {
                    if (clicked != -1)
                    {
                        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                        Vector3 v = GameObject.Find("Player").transform.position;
                        WM.Drop(playerInventory.Items[clicked], playerInventory.stacks[clicked], v);
                        playerInventory.Items[clicked] = -1;
                        playerInventory.stacks[clicked] = 0;
                        itemDepiction[clicked].GetComponent<UnityEngine.UI.Image>().sprite = null;
                        itemDepiction[clicked].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                        itemDepiction[clicked].transform.position = PrimaryMenuLocations[clicked];
                        clicked = -1;
                    }
                }
            }
        }
        else
        {
            if ((clicked == -1) && (clicked2 != -1))
            {
                if (otherInventory.Items[clicked2] != -1)
                {
                    clicked = clicked2;
                    clickedPrevLocation = SecondaryMenuItems[clicked].transform.position;
                }
            }
            else
            {
                if (clicked2 > -1)
                {
                    if (primary == prevprimary)
                    {

                        if(ItemSwap(clicked, clicked2, otherInventory, otherInventory, SecondaryMenuItems, SecondaryMenuItems, SecondaryMenuLocations, true))
                        clicked = -1;
                    }
                    else
                    {
                        if(ItemSwap(clicked, clicked2, playerInventory, otherInventory, itemDepiction, SecondaryMenuItems, PrimaryMenuLocations, true))
                        clicked = -1;
                    }
                }
                else
                {
                    if (clicked != -1)
                    {
                        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                        Vector3 v = GameObject.Find("Player").transform.position;
                        WM.Drop(otherInventory.Items[clicked], otherInventory.stacks[clicked], v);
                        otherInventory.Items[clicked] = -1;
                        otherInventory.stacks[clicked] = 0;
                        SecondaryMenuItems[clicked].GetComponent<UnityEngine.UI.Image>().sprite = null;
                        SecondaryMenuItems[clicked].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                        SecondaryMenuItems[clicked].transform.position = SecondaryMenuLocations[clicked];
                        clicked = -1;
                    }
                }
            }
        }
    }
}
