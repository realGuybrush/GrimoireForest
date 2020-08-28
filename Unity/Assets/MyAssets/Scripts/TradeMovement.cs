using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeMovement : InventoryMovement
{
    public int width2 = 3;
    public int height2 = 6;
    public float gridWidth2 = 150;
    public float gridHeight2 = 240;
    public float gridXOffset2 = 0.0f;

    override public void SetInv(Inventory PI, Inventory OI)
    {
        playerInventory = PI;
        otherInventory = OI;
        CalculateItemPositions();
    }
    override public void CalculateItemPositions()
    {
        if (width == 0 || height == 0 || width2 == 0 || height2 == 0)
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

        posX = -gridWidth2 / 2 + (gridWidth2 / width2) / 2;
        posY = gridHeight2 / 2 - (gridHeight2 / height2) / 2;
        buttonWidth = gridWidth2 / width2;
        buttonHeight = gridHeight2 / height2;
        SecondaryMenuLocations = new List<Vector3>();
        for (int i = 0; i < SecondaryMenuItems.Count; i++)
        {
            GameObject.Destroy(SecondaryMenuItems[i]);
        }
        SecondaryMenuItems = new List<GameObject>();
        I = (otherInventory.maxAmount > (width2 * height2)) ? (width2 * height2) : otherInventory.maxAmount;
        for (int i = 0; i < I; i++)
        {
            SecondaryMenuLocations.Add(new Vector3(transform.position.x + posX + buttonWidth * (i % width2) + gridXOffset2, transform.position.y + posY - buttonHeight * (int)(i / width2), 0.0f));
            SecondaryMenuItems.Add(Instantiate(itemInInvPrefab, transform));
            SecondaryMenuItems[i].transform.position = SecondaryMenuLocations[i];
        }
    }
}
