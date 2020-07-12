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
    private float bookWidth;
    private float bookHeight;
    public int width = 3;
    public int height = 6;
    public float gridWidth = 150;
    public float gridHeight = 240;
    public float gridXOffset = 0.0f;
    float MenuOffsetX;
    float MenuOffsetY;
    public GameObject itemInInvPrefab;

    List<GameObject> itemDepiction = new List<GameObject>();
    List<Vector3> PrimaryMenuLocations = new List<Vector3>();
    public List<GameObject> SecondaryMenuItems = new List<GameObject>();
    List<Vector3> SecondaryMenuLocations = new List<Vector3>();
    int clicked = -1;
    bool primary = true;

    public GameObject FloatingTile;
    int floatingItem = - 1;
    int floatingStack = 0;

    Vector3 clickedPrevLocation;
    bool prevprimary;


    private void Start()
    {
        GameObject Book = GameObject.Find("Book");
        bookWidth = Book.GetComponent<RectTransform>().rect.width;
        bookHeight = Book.GetComponent<RectTransform>().rect.height;
        MenuOffsetX = GameObject.Find("Menus").transform.position.x;
        MenuOffsetY = GameObject.Find("Menus").transform.position.y;
        for (int i = 0; i < SecondaryMenuItems.Count; i++)
        {
            SecondaryMenuLocations.Add(SecondaryMenuItems[i].transform.position);
        }
    }
    private bool GetMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = 0;
            return true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            clicked = 1;
            return true;
        }
        if (Input.GetMouseButtonDown(2))
        {
            clicked = 2;
            return true;
        }
        return false;
    }
    private void Update()
    {
        if (GetMouseButtonDown())
        {
            Clicked(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Floating())
        {
            FloatingTile.transform.position = Input.mousePosition;
        }
    }

    public void ShowHide()
    {
        if (Floating())
        {
            if (prevprimary)
            {
                ItemSwap((int)clickedPrevLocation.z, playerInventory, itemDepiction, false);
            }
            else
            {
                ItemSwap((int)clickedPrevLocation.z, otherInventory, SecondaryMenuItems, false);
            }
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
            PrimaryMenuLocations.Add(new Vector3(transform.position.x + posX + buttonWidth * (i % width) + gridXOffset, transform.position.y + posY - buttonHeight * (int)(i / width), 0.0f));
            itemDepiction.Add(Instantiate(itemInInvPrefab, transform));
            itemDepiction[i].transform.position = PrimaryMenuLocations[i];// transform.position + new Vector3(PrimaryMenuLocations[i].x, PrimaryMenuLocations[i].y, 0.0f);
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
        UpdateAllStacks(playerInventory, itemDepiction, otherInventory, SecondaryMenuItems);
    }
    private int CalculateButtonNumberByCoordinates(float X, float Y)
    {
        if ((X - gridXOffset >= (-gridWidth / 2)) && (X - gridXOffset <= (gridWidth / 2)) && (Y >= (-gridHeight / 2)) && (Y <= (gridHeight / 2)))
        {
            primary = true;
            return ((int)((X - gridXOffset + (gridWidth / 2)) / (gridWidth / width)) - ((int)((Y - (gridHeight / 2)) / (gridHeight / height))) * width);
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
        if ((X >= (-bookWidth / 2)) && (X <= (bookWidth / 2)) && (Y >= (-bookHeight / 2)) && (Y <= (bookHeight / 2)))
        {
            return -2;
        }
        return -1;
    }

    private void UpdateAllStacks(Inventory inv1, List<GameObject> buttons1, Inventory inv2, List<GameObject> buttons2)
    {
        for (int i = 0; i < inv1.stacks.Count; i++)
        {
            UpdateStack(buttons1[i], inv1.stacks[i]);
        }
        for (int i = 0; i < inv2.stacks.Count; i++)
        {
            UpdateStack(buttons2[i], inv2.stacks[i]);
        }
    }
    private void UpdateStack(GameObject button, int amount = 0)
    {
        if (amount > 1)
        {
            button.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = amount.ToString();
        }
        else
        {
            button.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }
    private bool Floating()
    {
        return (floatingItem > -1);
    }
    private bool ItemSwap(int index, Inventory inventory, List<GameObject> buttons, bool setget, bool onlyOneInDestination = false)
    {
        int maxStack;
        if (onlyOneInDestination)
        {
            maxStack = 1;
        }
        else
        {
            Item I;
            if (setget)
            {
                I = GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[inventory.Items[index]].GetComponent<Item>();
            }
            else
            {
                I = GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[floatingItem].GetComponent<Item>();
            }
            I.Start2();
            maxStack = I.itemValues.maxStack;
        }
        if (!setget)
        {
            if (inventory.Items[index] != floatingItem)
            {
                if (!onlyOneInDestination)
                {
                    int x = inventory.Items[index];
                    inventory.Items[index] = floatingItem;
                    floatingItem = x;
                    x = inventory.stacks[index];
                    inventory.stacks[index] = floatingStack;
                    floatingStack = x;
                    Sprite x2 = buttons[index].GetComponent<UnityEngine.UI.Image>().sprite;
                    buttons[index].GetComponent<UnityEngine.UI.Image>().sprite = FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = x2;
                    Color x3 = buttons[index].GetComponent<UnityEngine.UI.Image>().color;
                    buttons[index].GetComponent<UnityEngine.UI.Image>().color = FloatingTile.GetComponent<UnityEngine.UI.Image>().color;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().color = x3;
                    UpdateStack(buttons[index], inventory.stacks[index]);
                    return true;
                }
                else
                {
                    if (inventory.Items[index] == -1)
                    {
                        inventory.stacks[index] = 1;
                        floatingStack -= 1;
                        inventory.Items[index] = floatingItem;
                        buttons[index].GetComponent<UnityEngine.UI.Image>().sprite = FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite;
                        buttons[index].GetComponent<UnityEngine.UI.Image>().color = FloatingTile.GetComponent<UnityEngine.UI.Image>().color;
                        UpdateStack(buttons[index], inventory.stacks[index]);
                        if (floatingStack == 0)
                        {
                            floatingItem = -1;
                            FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                            FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); ;
                            return true;
                        }
                    }
                    return false;
                }
            }
            else
            {
                if (maxStack >= (inventory.stacks[index] + floatingStack))
                {
                    inventory.Items[index] = floatingItem;
                    floatingItem = -1;
                    inventory.stacks[index] += floatingStack;
                    floatingStack = 0;
                    buttons[index].GetComponent<UnityEngine.UI.Image>().sprite = FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                    buttons[index].GetComponent<UnityEngine.UI.Image>().color = FloatingTile.GetComponent<UnityEngine.UI.Image>().color;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); ;
                    UpdateStack(buttons[index], inventory.stacks[index]);
                    return true;
                }
                else
                {
                    floatingStack -= maxStack - inventory.stacks[index];
                    inventory.stacks[index] = maxStack;
                    UpdateStack(buttons[index], inventory.stacks[index]);
                    return false;
                }
            }
        }
        else
        {
            floatingItem = inventory.Items[index];
            inventory.Items[index] = -1;
            floatingStack = inventory.stacks[index];
            inventory.stacks[index] = 0;
            FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = buttons[index].GetComponent<UnityEngine.UI.Image>().sprite;
            buttons[index].GetComponent<UnityEngine.UI.Image>().sprite = null;
            FloatingTile.GetComponent<UnityEngine.UI.Image>().color = buttons[index].GetComponent<UnityEngine.UI.Image>().color;
            buttons[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); ;
            UpdateStack(buttons[index], inventory.stacks[index]);
            return true;
        }
    }
    /*private bool ItemSwap(int index1, int index2, Inventory inventory1, Inventory inventory2, List<GameObject> buttons1, List<GameObject> buttons2, List<Vector3> locations, bool onlyOneInDestination = false)
    {
        int maxStack;
        if ((index1 == index2) && (primary == prevprimary))
        {
            return true;
        }
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
            if (!onlyOneInDestination)
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
                UpdateStack(buttons1[index1], inventory1.stacks[index1]);
                UpdateStack(buttons2[index2], inventory2.stacks[index2]);
                return true;
            }
            else
            {
                inventory2.stacks[index2] = 1;
                inventory1.stacks[index1] -= 1;
                inventory2.Items[index2] = inventory1.Items[index1];
                buttons2[index2].GetComponent<UnityEngine.UI.Image>().sprite = buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite;
                buttons2[index2].GetComponent<UnityEngine.UI.Image>().color = buttons1[index1].GetComponent<UnityEngine.UI.Image>().color;
                buttons1[index1].transform.position = locations[index1];
                UpdateStack(buttons1[index1], inventory1.stacks[index1]);
                UpdateStack(buttons2[index2], inventory2.stacks[index2]);
                if (inventory1.stacks[index1] == 0)
                {
                    inventory1.Items[index1] = -1;
                    buttons1[index1].GetComponent<UnityEngine.UI.Image>().sprite = null;
                    buttons1[index1].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); ;
                    return true;
                }
                primary = prevprimary;
                return false;
            }
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
                UpdateStack(buttons1[index1], inventory1.stacks[index1]);
                UpdateStack(buttons2[index2], inventory2.stacks[index2]);
                return true;
            }
            else
            {
                inventory1.stacks[index1] -= maxStack - inventory2.stacks[index2];
                inventory2.stacks[index2] = maxStack;
                UpdateStack(buttons1[index1], inventory1.stacks[index1]);
                UpdateStack(buttons2[index2], inventory2.stacks[index2]);
                primary = prevprimary;
                return false;
            }
        }
    }
    */
    public void Clicked(float X, float Y)
    {
        int clicked2 = CalculateButtonNumberByCoordinates(X - MenuOffsetX, Y - MenuOffsetY);
        if (clicked2 == -2)
        {
            //primary = prevprimary;
            return;
        }
        if (Floating())
        {
            if (clicked2 == -1)
            {
                WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                Vector3 v = GameObject.Find("Player").transform.position;
                WM.Drop(floatingItem, floatingStack, v);
                floatingItem = -1;
                floatingStack = 0;
                FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
            else
            {
                if (primary)
                {
                    ItemSwap(clicked2, playerInventory, itemDepiction, false);
                }
                else
                {
                    ItemSwap(clicked2, otherInventory, SecondaryMenuItems, false, true);
                }
            }
        }
        else
        {
            if (clicked2 != -1)
            {
                if (primary)
                {
                    if (playerInventory.Items[clicked2] != -1)
                    {
                        ItemSwap(clicked2, playerInventory, itemDepiction, true);
                        clickedPrevLocation = new Vector3(itemDepiction[clicked2].transform.position.x, itemDepiction[clicked2].transform.position.y, clicked2);
                        prevprimary = primary;
                    }
                }
                else
                {
                    if (otherInventory.Items[clicked2] != -1)
                    {
                        ItemSwap(clicked2, otherInventory, SecondaryMenuItems, true);
                        clickedPrevLocation = new Vector3(SecondaryMenuItems[clicked2].transform.position.x, SecondaryMenuItems[clicked2].transform.position.y, clicked2);
                        prevprimary = primary;
                    }
                }
            }
        }
        /*if (primary)
        {
            if (Floating())
            {
                if (clicked2 != -1)
                {
                    if (playerInventory.Items[clicked2] != -1)
                    {
                        clickedPrevLocation = new Vector3(itemDepiction[clicked].transform.position.x, itemDepiction[clicked].transform.position.y, clicked);
                    }
                }
            }
            else
            {
                if (clicked2 > -1)
                {
                    ItemSwap(clicked2, playerInventory, itemDepiction, true);
                }
                else
                {
                    if (Floating())
                    {
                        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                        Vector3 v = GameObject.Find("Player").transform.position;
                        WM.Drop(floatingItem, floatingStack, v);
                        floatingItem = -1;
                        floatingStack = 0;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                    }
                }
            }
        }
        else
        {
            if (Floating())
            {
                if (clicked2 != -1)
                {
                    if (otherInventory.Items[clicked2] != -1)
                    {
                        clickedPrevLocation = new Vector3(SecondaryMenuItems[clicked].transform.position.x, SecondaryMenuItems[clicked].transform.position.y, clicked);
                    }
                }
            }
            else
            {
                if (clicked2 > -1)
                {
                    ItemSwap(clicked2, otherInventory, SecondaryMenuItems, true);
                }
                else
                {
                    if (Floating())
                    {
                        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                        Vector3 v = GameObject.Find("Player").transform.position;
                        WM.Drop(floatingItem, floatingStack, v);
                        floatingItem = -1;
                        floatingStack = 0;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                    }
                }
            }
        }*/
    }
}
