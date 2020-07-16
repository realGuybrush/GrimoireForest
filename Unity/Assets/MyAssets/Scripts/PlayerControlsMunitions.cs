using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    //maybe remove variety of weapons and simplify this shit?
    public GameObject Gun;
    public GameObject Sword;
    public GameObject Spear;
    public GameObject Arrow;
    public GameObject Rod;

    public GameObject MagicArtefact;
    public GameObject Bow;

    public GameObject Helmet;
    public GameObject Armor;
    public GameObject Pants;
    public GameObject Boots;
    public GameObject Gloves;
    public GameObject Shield;
    public GameObject Weapon;

    public Inventory munitions = new Inventory(11);
    public int weaponSlotNumber = 0;
    //public int spellSlotNumber = 0;

    private void CheckWeaponSpell()
    {
        if ((Weapon != null) && (munitions.Items[6 + weaponSlotNumber] == -1))
        {
            RemoveWeapon();
        }
        else
        {
            if ((Weapon != null) && (Weapon.GetComponent<Item>().itemValues.number != munitions.Items[weaponSlotNumber + 6]))
            {
                RemoveWeapon();
            }
            if ((Weapon == null) && (munitions.Items[weaponSlotNumber + 6] != -1))
            {
                GameObject G = Instantiate(GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[munitions.Items[weaponSlotNumber + 6]]);
                G.GetComponent<Item>().Start2();
                SetWeapon(G);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (((munitions.Items[i + 6] == -1) && (Arms.GetComponent<ArmsDepiction>().ArmsAndSpells[i].GetComponent<UnityEngine.UI.Image>().sprite != null)) ||
                ((munitions.Items[i + 6] != -1) && (Arms.GetComponent<ArmsDepiction>().ArmsAndSpells[i].GetComponent<UnityEngine.UI.Image>().sprite == null)))
            {
                Arms.GetComponent<ArmsDepiction>().Update2(i);
            }
        }
    }

    public bool SetMunitions(GameObject munition)
    {
        //think this through later
        switch (munition.GetComponent<Item>().itemValues.type)
        {
            case "Helmet":
                Helmet = munition;
                Helmet.transform.SetParent(Gun.transform);
                break;
            case "Armor":
                Armor = munition;
                Armor.transform.SetParent(Sword.transform);
                break;
            case "Pants":
                Weapon = munition;
                Weapon.transform.SetParent(Spear.transform);
                break;
            case "Boots":
                Weapon = munition;
                Weapon.transform.SetParent(Arrow.transform);
                break;
            case "Gloves":
                Weapon = munition;
                Weapon.transform.SetParent(Rod.transform);
                break;
            default:
                return SetWeapon(munition);
        }
        //Weapon.transform.localPosition = Weapon.GetComponent<Item>().positionOnHand;
        //Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
        return true;
    }
    public bool SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            switch (weapon.GetComponent<Item>().itemValues.type)
            {
                case "Gun":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Gun.transform);
                    break;
                case "Sword":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Sword.transform);
                    break;
                case "Spear":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Spear.transform);
                    break;
                case "Arrow":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Arrow.transform);
                    break;
                case "Rod":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Rod.transform);
                    break;
                case "MagicArtefact":
                    Weapon = weapon;
                    Weapon.transform.SetParent(MagicArtefact.transform);
                    break;
                case "Bow":
                    Weapon = weapon;
                    Weapon.transform.SetParent(Bow.transform);
                    break;
                default:
                    return false;
            }
        }
        Weapon.transform.localPosition = Weapon.GetComponent<Item>().positionOnHand;
        Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
        return true;
    }
    public void RemoveWeapon()
    {

        switch (Weapon.GetComponent<Item>().itemValues.type)
        {
            case "Gun":
                foreach (Transform child in Gun.transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case "Sword":
                foreach (Transform child in Sword.transform)
                {
                    Destroy(child);
                }
                break;
            case "Spear":
                foreach (Transform child in Spear.transform)
                {
                    Destroy(child);
                }
                break;
            case "Arrow":
                foreach (Transform child in Arrow.transform)
                {
                    Destroy(child);
                }
                break;
            case "Rod":
                foreach (Transform child in Rod.transform)
                {
                    Destroy(child);
                }
                break;
            case "MagicArtefact":
                foreach (Transform child in MagicArtefact.transform)
                {
                    Destroy(child);
                }
                break;
            case "Bow":
                foreach (Transform child in Bow.transform)
                {
                    Destroy(child);
                }
                break;
            default:
                break;
        }
        Weapon = null;
    }
    public string GetAttackType(int attackIndex)
    {
        switch (attackIndex)
        {
            case 1:
                return Weapon.GetComponent<Item>().itemValues.atk1;
            case 2:
                return Weapon.GetComponent<Item>().itemValues.atk2;
            case 3:
                return Weapon.GetComponent<Item>().itemValues.kick;
            default:
                break;
        }
        return "";
    }
}
