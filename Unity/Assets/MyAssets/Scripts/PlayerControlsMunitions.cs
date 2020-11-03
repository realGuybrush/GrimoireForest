using System;
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
    public Inventory spells = new Inventory(5);
    public int weaponSlotNumber = 0;
    public int spellSlotNumber = 0;
    public Inventory spellInventory = new Inventory();

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
                //G.GetComponent<Item>().Start2();
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
        for (int i = 5; i < 10; i++)
        {
            if (((spells.Items[i - 5] == -1) && (Arms.GetComponent<ArmsDepiction>().ArmsAndSpells[i].GetComponent<UnityEngine.UI.Image>().sprite != null)) ||
                ((spells.Items[i - 5] != -1) && (Arms.GetComponent<ArmsDepiction>().ArmsAndSpells[i].GetComponent<UnityEngine.UI.Image>().sprite == null)))
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
        if (Weapon.GetComponent<Rigidbody2D>() != null)
        {
            Weapon.GetComponent<Rigidbody2D>().simulated = false;// .isKinematic = false;
            //Weapon.GetComponent<Rigidbody2D>().collisionDetectionMode = false;
        }
        Weapon.GetComponent<Item>().SetPenalty(CalcWeaponPenalty(Weapon.GetComponent<Item>().strReq));
        //BackFire(Weapon.GetComponent<Item>().strReq);
        Weapon.transform.localPosition = Weapon.GetComponent<Item>().positionOnHand;
        Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
        RecalcCharacteristics(GetAttackType(1));
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
                    Destroy(child.gameObject);
                }
                break;
            case "Spear":
                foreach (Transform child in Spear.transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case "Arrow":
                foreach (Transform child in Arrow.transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case "Rod":
                foreach (Transform child in Rod.transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case "MagicArtefact":
                foreach (Transform child in MagicArtefact.transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case "Bow":
                foreach (Transform child in Bow.transform)
                {
                    Destroy(child.gameObject);
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
    public Buff GetBuff(int attackIndex)
    {
        return Weapon.GetComponent<Item>().itemValues.GetBuff(attackIndex);
    }

    public void BackFire(int weaponStr=0)
    {
        Vector2 forceAngle = CalcBackFireXY();
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceAngle.x*25.0f*(1.0f-CalcWeaponPenalty(weaponStr)), forceAngle.y * 25.0f * (1.0f - CalcWeaponPenalty(weaponStr))));
        SetBackFireAngle(CalcWeaponPenalty(weaponStr));
    }

    public Vector2 CalcBackFireXY()
    {
        float dX, dY;
        float gipotenuza;
        Vector2 dir;
        //it should be middle of the back bone, but here would be base, because whatever
        dX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x;
        dY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - this.transform.position.y;
        gipotenuza = (float)Math.Sqrt(dX * dX + dY * dY);
        float tempY = (float)(dY / gipotenuza);
        if ((tempY > -0.05f) && !BasicCheckMidAir())
            tempY = 0.0f;
        dir = new Vector2 ((float)(dX / gipotenuza), tempY);
        return dir;
    }
}
