using System;
using UnityEngine;

public class MenusTrigger : MonoBehaviour {
    [SerializeField]
    private MunitionsMovement Inventory;

    [SerializeField]
    private SpellMovement Spells;

    //todo: merge MenuController and MenusTrigger
    [SerializeField]
    public MenuController Menus;

    [SerializeField]
    private InventoryMovement Trade;

    [SerializeField]
    private TalkMovement Talk;

    public Action OnCloseMenu = delegate { };

    public bool IsMenuActive() {
        return Menus.gameObject.activeSelf;
    }

    public void InitAll() {
        Inventory.GetComponent<MunitionsMovement>().Initialize();
        Spells.GetComponent<SpellMovement>().Initialize();
        Trade.GetComponent<InventoryMovement>().Initialize();
        ShowInv(false);
        ShowSpell(false);
        ShowTrade(false);
    }

    public void SetInv(Inventory inv1, Inventory inv2) {
        Inventory.SetInv(inv1, inv2);
        Inventory.ShowHide();
    }

    public void ShowInv(bool show) {
        Inventory.gameObject.SetActive(show);
    }

    public void SetSpell(Inventory inv1, Inventory inv2) {
        Spells.SetInv(inv1, inv2);
        Spells.ShowHide();
    }

    public void ShowSpell(bool show) {
        Spells.gameObject.SetActive(show);
    }

    public void SetTrade(Inventory inv1, Inventory inv2) {
        Trade.SetInv(inv1, inv2);
        Trade.ShowHide();
    }

    public void ShowTrade(bool show) {
        Trade.gameObject.SetActive(show);
    }

    public void ShowMenu(bool show) {
        Menus.gameObject.SetActive(show);
    }

    public bool EscapeMenu() {
        bool escape = Menus.Escape();
        if (escape) {
            ShowMenu(false);
            OnCloseMenu?.Invoke();
        }
        return escape;
    }
}
