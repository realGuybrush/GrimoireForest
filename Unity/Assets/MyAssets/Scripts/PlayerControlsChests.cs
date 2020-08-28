using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public List<GameObject> chest;
    int chosenChestIndex = 0;
    public void IncludeChest(GameObject newP)
    {
        chest.Add(newP);
    }

    public void ExcludeChest(GameObject ExcP)
    {
        if (chest.Contains(ExcP))
        {
            chest.Remove(ExcP);
        }
    }

    public void ChooseChest()
    {
        chosenChestIndex = 0;
        for (int i = 0; i < chest.Count; i++)
        {
            if (!chest[i].GetComponent<Chest>().realChest)
            {
                chosenChestIndex = i;
                break;
            }
        }
    }
    public void DeleteEmptyDrops()
    {
        for (int i = chest.Count - 1; i >= 0; i--)
        {
            if ((!chest[i].GetComponent<Chest>().realChest) && (chest[i].GetComponent<Chest>().IsEmpty()))
            {
                GameObject.Destroy(chest[i]);
                //chest.RemoveAt(i);
            }
        }
    }
}
