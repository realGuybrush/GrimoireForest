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
    List<float> buttonX = new List<float>();
    List<float> buttonY = new List<float>();
    List<GameObject> itemDepiction = new List<GameObject>();
    int clicked = -1;
    public GameObject itemInInvPrefab;

    private void Update()
    {
        if (clicked > -1)
        {
            if (itemDepiction.Count > clicked)
            {
                itemDepiction[clicked].transform.position = Input.mousePosition;
            }
        }
    }

    public void ShowHide()
    {
        GameObject B = GameObject.Find("Book");
        UploadToHUD();
        if (B.transform.position.y < 10000)
        {
            B.transform.position = new Vector3(B.transform.position.x, B.transform.position.y + 10000.0f, B.transform.position.z);
        }
        else
        {
            B.transform.position = new Vector3(B.transform.position.x, B.transform.position.y - 10000.0f, B.transform.position.z);
        }
    }
    public void OnMouseDown()
    {
        Clicked(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void SetInv(Inventory PI)//, Inventory OI, List<GameObject> WL
    {
        playerInventory = PI;
        //otherInventory = OI;
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
        buttonX = new List<float>();
        buttonY = new List<float>();
        for (int i = 0; i < itemDepiction.Count; i++)
        {
            GameObject.Destroy(itemDepiction[i]);
        }
        itemDepiction = new List<GameObject>();
        int I = (playerInventory.maxAmount > (width * height)) ? (width * height) : playerInventory.maxAmount;
        for (int i = 0; i < I; i++)
        {
            buttonX.Add(posX + buttonWidth * (i % width));
            buttonY.Add(posY - buttonHeight * (int)(i / width));
            itemDepiction.Add(Instantiate(itemInInvPrefab, transform));
            itemDepiction[i].transform.position = transform.position + new Vector3(buttonX[i] + xOffset, buttonY[i], 0.0f);
        }
    }
    public void UploadToHUD()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        if (playerInventory != null)
        {
            for (int i = 0; i < buttonX.Count; i++)
            {
                itemDepiction[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
            }
            int I = (playerInventory.maxAmount > buttonX.Count) ? buttonX.Count : playerInventory.maxAmount;
            for (int i = 0; i < I; i++)
            {
                if (playerInventory.Items.Count > i)
                {
                    if (playerInventory.Items[i] >= 0)
                    {
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().sprite = WM.ItemPrefabs[playerInventory.Items[i]].GetComponent<SpriteRenderer>().sprite;
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().SetNativeSize();
                        Rect R = itemDepiction[i].GetComponent<UnityEngine.UI.Image>().rectTransform.rect;
                        float shrinkCoeff = (R.width < R.height) ? (R.height / 30) : (R.width / 30);
                        R.width /= shrinkCoeff;
                        R.height /= shrinkCoeff; 
                        itemDepiction[i].GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta = new Vector2(R.width, R.height);//shrink it
                    }
                }
            }
        }
    }
    private int CalculateButtonNumberByCoordinates(float X, float Y)
    {
        if ((X >= (-gridWidth / 2)) && (X <= (gridWidth / 2)) && (Y >= (-gridHeight / 2)) && (Y <= (gridHeight / 2)))
        {
            return (int)((X+(gridWidth/2)%(gridWidth/width))+ (Y + (gridHeight / 2) % (gridHeight / height))*width);
        }
        return -1;
    }
    public void Clicked(float X, float Y)
    {
        int clicked2 = CalculateButtonNumberByCoordinates(X, Y);
        if (clicked == -1)
        {
            clicked = clicked2;
        }
        else
        {
            if (clicked2 > -1)
            {
                int x = playerInventory.Items[clicked];
                playerInventory.Items[clicked] = playerInventory.Items[clicked2];
                playerInventory.Items[clicked2] = x;
                x = playerInventory.stacks[clicked];
                playerInventory.stacks[clicked] = playerInventory.stacks[clicked2];
                playerInventory.stacks[clicked2] = x;
            }
            else
            {
                WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
                Vector3 v = GameObject.Find("Player").transform.position;
                WM.Drop(playerInventory.Items[clicked], playerInventory.stacks[clicked], v);
                playerInventory.Items[clicked] = -1;
                playerInventory.stacks[clicked] = 0;
                itemDepiction[clicked].GetComponent<UnityEngine.UI.Image>().sprite = null;
                itemDepiction[clicked].GetComponent<UnityEngine.UI.Image>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
        }
    }
}
