using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InventoryMovement : MonoBehaviour
{
    public WorldManagement WM;
    public GameObject Player;
    public Inventory playerInventory;
    public Inventory otherInventory;
    public float bookWidth;
    public float bookHeight;
    public int width = 3;
    public int height = 6;
    public float gridWidth = 150;
    public float gridHeight = 240;
    public float gridXOffset = 0.0f;
    public float MenuOffsetX;
    public float MenuOffsetY;
    public GameObject itemInInvPrefab;

    public List<GameObject> itemDepiction = new List<GameObject>();
    public List<Vector3> PrimaryMenuLocations = new List<Vector3>();
    public List<GameObject> SecondaryMenuItems = new List<GameObject>();
    public List<Vector3> SecondaryMenuLocations = new List<Vector3>();
    public int clicked = -1;
    public bool primary = true;

    public GameObject FloatingTile;
    public int floatingItem = - 1;
    public int floatingStack = 0;

    public Vector3 clickedPrevLocation;
    public bool prevprimary;


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
        WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        Player = GameObject.Find("Player");
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
        if (WM == null)
        {
            WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        }
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }
    }

    public void ShowHide(bool show = true)
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

    public void SetInv(Inventory PI, Inventory OI)
    {
        playerInventory = PI;
        otherInventory = OI;
        CalculateItemPositions();
    }

    public void CalculateItemPositions()
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
            itemDepiction[i].transform.position = PrimaryMenuLocations[i];
        }
    }
    public void UploadToHUD()
    {
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
    public int CalculateButtonNumberByCoordinates(float X, float Y)
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

    public void UpdateAllStacks(Inventory inv1, List<GameObject> buttons1, Inventory inv2, List<GameObject> buttons2)
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
    public void UpdateStack(GameObject button, int amount = 0)
    {
        if (amount > 1)
        {
            button.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = amount.ToString();
        }
        else
        {
            button.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "";
        }
        if (floatingStack > 1)
        {
            FloatingTile.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = floatingStack.ToString();
        }
        else
        {
            FloatingTile.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }
    public bool Floating()
    {
        return (floatingItem > -1);
    }
    public bool ItemSwap(int index, Inventory inventory, List<GameObject> buttons, bool setget, bool onlyOneInDestination = false)
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
                    if (clicked == 0)
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
                        if (clicked == 1)
                        {
                            if (inventory.Items[index] == -1)
                            {
                                inventory.stacks[index] += 1;
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
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
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
                if (clicked == 0)
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
                else
                {
                    //if(clicked == 1))
                    if (inventory.stacks[index] < maxStack)
                    {
                        inventory.stacks[index] += 1;
                        floatingStack -= 1;
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
        }
        else
        {
            switch(clicked)
            {
                case 0:
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
                case 1:
                    floatingStack = Mathf.RoundToInt(((float)inventory.stacks[index])/2 - 0.01f);
                    inventory.stacks[index] = Mathf.RoundToInt(((float)inventory.stacks[index]) / 2 + 0.01f);
                    if (floatingStack == 0)
                    {
                        floatingStack = 1;
                        inventory.stacks[index] = 0;
                    }
                    floatingItem = inventory.Items[index];
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = buttons[index].GetComponent<UnityEngine.UI.Image>().sprite;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().color = buttons[index].GetComponent<UnityEngine.UI.Image>().color;
                    UpdateStack(buttons[index], inventory.stacks[index]);
                    if (inventory.stacks[index] == 0)
                    {
                        inventory.Items[index] = -1;
                        buttons[index].GetComponent<UnityEngine.UI.Image>().sprite = null;
                        buttons[index].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                        return false;
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
    public virtual void Clicked(float X, float Y)
    {
        int clicked2 = CalculateButtonNumberByCoordinates(X - MenuOffsetX, Y - MenuOffsetY);
        if (clicked2 == -2)
        {
            return;
        }
        if (Floating())
        {
            if (clicked2 == -1)
            {
                if (clicked == 0)
                {
                    Vector3 v = Player.transform.position;
                    WM.Drop(floatingItem, floatingStack, v);
                    floatingItem = -1;
                    floatingStack = 0;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                    FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                }
                if (clicked == 1)
                {
                    Vector3 v = Player.transform.position;
                    WM.Drop(floatingItem, 1, v);
                    floatingStack -= 1;
                    UpdateStack(itemDepiction[0], playerInventory.stacks[0]);
                    if (floatingStack == 0)
                    {
                        floatingItem = -1;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().sprite = null;
                        FloatingTile.GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
                    }
                }
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
    }
}
