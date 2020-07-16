using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusTrigger : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Spells;
    public GameObject Menus;
    public GameObject Trade;

    public void SetInv(Inventory inv1, Inventory inv2)
    {
        Inventory.GetComponent<InventoryMovement>().SetInv(inv1, inv2);
        Inventory.GetComponent<InventoryMovement>().ShowHide();
    }
    public void ShowInv(bool show)
    {
        Inventory.SetActive(show);
    }
    public void SetSpell(Inventory inv1, Inventory inv2)
    {
        Spells.GetComponent<InventoryMovement>().SetInv(inv1, inv2);
        Spells.GetComponent<InventoryMovement>().ShowHide();
    }
    public void ShowSpell(bool show)
    {
        Spells.SetActive(show);
    }
    public void ShowMenu(bool show)
    {
        Menus.SetActive(show);
    }
}
