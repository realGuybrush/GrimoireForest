using UnityEngine;

public class Item : MonoBehaviour
{
    private ContactPoint2D grabPoint1;
    private ContactPoint2D grabPoint2;
    public Vector3 positionOnHand = new Vector3(1.630001f, -0.06000054f, 0.0f);
    public ItemCharacteristics itemValues;
    private Collider2D thisCollider;
    public Sprite InventoryImage;
    private bool set;
    private int atkType;
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
        itemValues.maxStack = 3;
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
        itemValues.SetProjectiles("Prefabs\\Projectiles\\Bullet", "", "");
    }
    public void Attack(bool value, int type)
    {
        set = value;
        atkType = type;
    }
    public void Shoot()
    {
        Start2();
        if (itemValues.GetProjectile(atkType) != null)
        {
            set = false;
            GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType));// add position
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);// make it normal
            //set any gun buffs for bullet.GetComponent<Projectile>().debuff
        }
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
        else
        {
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                collision.gameObject.GetComponent<Health>().Substract(itemValues.GetBuff(atkType).atk);
            }
            if (collision.gameObject.GetComponent<BasicMovement>() != null)
            {
                collision.gameObject.GetComponent<BasicMovement>().thisHealth.Substract(itemValues.GetBuff(atkType).atk);
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
