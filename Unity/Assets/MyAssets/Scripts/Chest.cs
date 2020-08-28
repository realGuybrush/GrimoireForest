using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Inventory inventory = new Inventory(54);
    public bool realChest = true;
    // Start is called before the first frame update
    void Start()
    {
        inventory.Items[0] = 0;
        inventory.stacks[0] = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInventory(Inventory inv)
    {
        for (int i = 0; i < inv.Items.Count && i < inventory.Items.Count; i++)
        {
            inventory.Items[i] = inv.Items[i];
            inventory.stacks[i] = inv.stacks[i];
        }
    }
    public bool IsEmpty()
    {
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (inventory.Items[i] != -1)
            {
                return false;
            }
        }
        return true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludeChest(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludeChest(this.gameObject);
        }
    }
}
