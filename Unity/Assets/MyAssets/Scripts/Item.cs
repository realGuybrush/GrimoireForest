using UnityEngine;

public class Item : MonoBehaviour
{
    private ContactPoint2D grabPoint1;
    private ContactPoint2D grabPoint2;
    public Vector3 positionOnHand = new Vector3(1.630001f, -0.06000054f, 0.0f);
    public ItemCharacteristics itemValues;
    private Collider2D thisCollider;
    public Sprite InventoryImage;
    //onCollision
    // search for hp script and extract hp.
    void Start()
    {
        thisCollider = this.gameObject.GetComponent<Collider2D>();
        //instead of this, items values should be loaded by worldmanager
        itemValues = new ItemCharacteristics();
        itemValues.number = 0;
        itemValues.type = "Gun";
        itemValues.atk1 = "Atk2";
        itemValues.atk2 = "Atk7";
        itemValues.kick = "Atk4";
        itemValues.maxStack = 2;
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
    }
    public void Start2()
    {
        thisCollider = this.gameObject.GetComponent<Collider2D>();
        //instead of this, items values should be loaded by worldmanager
        itemValues = new ItemCharacteristics();
        itemValues.number = 0;
        itemValues.type = "Gun";
        itemValues.atk1 = "Atk2";
        itemValues.atk2 = "Atk7";
        itemValues.kick = "Atk4";
        itemValues.maxStack = 2;
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.transform.parent == null)
        {
            if (collision.gameObject.GetComponent<PlayerControls>() != null)
            {
                collision.gameObject.GetComponent<PlayerControls>().IncludePickable(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.transform.parent == null)
        {
            if (collision.gameObject.GetComponent<PlayerControls>() != null)
            {
                collision.gameObject.GetComponent<PlayerControls>().ExcludePickable(this.gameObject);
            }
        }
    }
}
