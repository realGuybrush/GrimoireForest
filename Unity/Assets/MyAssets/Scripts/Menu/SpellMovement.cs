using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : InventoryMovement
{
    public bool isAllowed(int item, int index)
    {
        if (WM.ItemPrefabs[item].GetComponent<Item>().itemValues.type == "Spell")
        {
            return true;
        }
        return false;
    }
    public override void Clicked(float X, float Y)
    {
        int clicked2 = CalculateButtonNumberByCoordinates(X - MenuOffsetX, Y - MenuOffsetY);
        if (clicked2 < 0)
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
                    if (isAllowed(floatingItem, clicked2))
                    {
                        ItemSwap(clicked2, otherInventory, SecondaryMenuItems, false, true);
                    }
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
