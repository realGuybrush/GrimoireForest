using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public int maxAmount = 18;
	public List<int> Items = new List<int>();
    public List<int> stacks = new List<int>();
    public void Start()
    {
        for (int i = Items.Count; i < maxAmount; i++)
        {
            Items.Add(-1);
            stacks.Add(0);
        }
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
