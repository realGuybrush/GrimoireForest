using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public int maxAmount = 10;
    public int width = 10;
	public List<int> Items = new List<int>();
    public List<int> stacks = new List<int>();
    public bool Add1(Item newItem)
    {
        for (int i = 0; i < maxAmount; i++)
        {
            if (i >= Items.Count)
            {
                Items.Add(newItem.itemValues.number);
                stacks.Add(1);
                return true;
            }
            else
            {
                if (Items[i] == newItem.itemValues.number)
                {
                    if (stacks[i] < newItem.itemValues.maxStack)
                    {
                        stacks[i]++;
                        return true;
                    }
                }
            }
        }
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
