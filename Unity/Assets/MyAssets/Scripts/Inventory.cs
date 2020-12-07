
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory 
{
    public int maxAmount = 54;
	public List<int> Items = new List<int>();
    public List<int> stacks = new List<int>();

    public Inventory(int maxAmount = 54)
    {
        for (int i = Items.Count; i < maxAmount; i++)
        {
            Items.Add(-1);
            stacks.Add(0);
        }
    }
    public void Start()
    {
    }
    public bool Add1(Item newItem)
    {
        for (int i = 0; i < maxAmount; i++)
        {
            if ((Items[i] == newItem.itemValues.number) || (Items[i] == -1))
            {
                Items[i] = newItem.itemValues.number;
                if (stacks[i] < newItem.itemValues.maxStack)
                {
                    stacks[i]++;
                    return true;
                }
            }
        }
        return false;
    }

    public bool Remove(int itemIndex, int amount)
    {
        if (MoreThanThisLeft(itemIndex, amount))
        {
            if (amount == 0)
                return true;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == itemIndex)
                {
                    if (stacks[i] <= amount)
                    {
                        amount -= stacks[i];
                        stacks[i] = 0;
                        Items[i] = -1;
                    }
                    else
                    {
                        stacks[i] -= amount;
                        return true;
                    }
                }
                if (amount == 0)
                    return true;
            }
        }
        return false;
    }

    public bool MoreThanThisLeft(int itemIndex, int amount)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == itemIndex)
            {
                amount -= stacks[i];
                if (amount <= 0)
                {
                    return true;
                }
            }
        }
        if (amount <= 0)
            return true;
        return false;
    }
    public void Load(List<int> invemtory, List<int> Stacks)
	{
		Items = invemtory;
        stacks = Stacks;
	}
	public void Load(Inventory Inv)
	{
		Items = Inv.Items;
        stacks = Inv.stacks;
    }


}
