using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusTrigger : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Spells;
    public GameObject Menus;
    public GameObject Trade;
    public GameObject Talk;

    public bool IsMenuActive()
    {
        return Menus.activeSelf;
    }
    public void InitAll()
    {
        Inventory.GetComponent<MunitionsMovement>().Initialize();
        Spells.GetComponent<SpellMovement>().Initialize();
        Trade.GetComponent<InventoryMovement>().Initialize();
    }
    public void SetInv(Inventory inv1, Inventory inv2)
    {
        Inventory.GetComponent<MunitionsMovement>().SetInv(inv1, inv2);
        Inventory.GetComponent<MunitionsMovement>().ShowHide();
    }
    public void ShowInv(bool show)
    {
        Inventory.SetActive(show);
    }
    public void SetSpell(Inventory inv1, Inventory inv2)
    {
        Spells.GetComponent<SpellMovement>().SetInv(inv1, inv2);
        Spells.GetComponent<SpellMovement>().ShowHide();
    }
    public void ShowSpell(bool show)
    {
        Spells.SetActive(show);
    }
    public void SetTrade(Inventory inv1, Inventory inv2)
    {
        Trade.GetComponent<InventoryMovement>().SetInv(inv1, inv2);
        Trade.GetComponent<InventoryMovement>().ShowHide();
    }
    public void ShowTrade(bool show)
    {
        Trade.SetActive(show);
    }
    public void ShowMenu(bool show)
    {
        Menus.SetActive(show);
    }
    public bool EscapeMenu()
    {
        bool escape = Menus.GetComponent<MenuController>().Escape();
        if (escape)
        {
            ShowMenu(false);
            GameObject.Find("WorldManager").GetComponent<WorldManagement>().UnStopTime();
        }
        return escape;
    }
}
