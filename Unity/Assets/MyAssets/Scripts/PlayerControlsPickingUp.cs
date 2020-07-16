using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public void IncludePickable(GameObject newP)
    {
        PickableItem.Add(newP);
    }

    public void ExcludePickable(GameObject ExcP)
    {
        if (PickableItem.Contains(ExcP))
        {
            PickableItem.Remove(ExcP);
        }
    }

    public bool PickUp(List<GameObject> item)
    {
        if (item.Count == 0)
            return false;
        //absolutely needed: without it item[0] will sometimes be null after picking up previous item, and won't be deleted for soe reason
        //if you simply put item.RemoveAt(0) after deletion of item, however, two items will be removed from List! magic
        if (item[0] == null)
        {
            item.RemoveAt(0);
            if (item.Count == 0)
                return false;
        }
        Item I = item[0].GetComponent<Item>();
        if (I == null)
            return false;
        if (munitions.Items.Contains(-1))
        {
            if (PickUpMunitions(item[0]))
            {
                GameObject.Destroy(item[0]);
                return true;
            }
            else
            {
                if (inventory.Add1(I))
                {
                    GameObject.Destroy(item[0]);
                    return true;
                }
            }
        }
        else
        {
            if (inventory.Add1(I))
            {
                GameObject.Destroy(item[0]);
                return true;
            }
        }
        return false;
    }
    public bool PickUpMunitions(GameObject munition)
    {
        return PickUpWeapons(munition);
    }

    public bool PickUpWeapons(GameObject munition)
    {
        int i;
        for (i = 6; i < 11; i++)
        {
            if (munitions.Items[i] == -1)
            {
                break;
            }
        }
        if (i == 11)
        {
            return false;
        }
        for (i = 6; i < 11; i++)
        {
            if (munitions.Items[i] == -1)
            {
                munitions.Items[i] = munition.GetComponent<Item>().itemValues.number;
                munitions.stacks[i] = 1;
                break;
            }
        }
        Arms.GetComponent<ArmsDepiction>().Update2(i - 6);
        return true;
    }
}
