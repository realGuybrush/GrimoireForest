using UnityEngine;

public class Item : MonoBehaviour
{
    private ContactPoint2D grabPoint1;
    private ContactPoint2D grabPoint2;
    private Vector3 projectilePosition;
    private Vector3 projectilePosition1;
    public Vector3 positionOnHand = new Vector3(1.630001f, -0.06000054f, 0.0f);
    public float projectileVelocity = 50.0f;
    public ItemCharacteristics itemValues;
    private Collider2D thisCollider;
    public Sprite InventoryImage;
    private bool set;
    private bool setShoot;
    private int atkType;
    private string masterName;
    public int projectileIndex;
    public int projectilePerShot;
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
        projectileIndex = 2;
        projectilePerShot = 0;
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
        masterName = "Player";
    }
    public void Start2()
    {
        projectilePosition = this.gameObject.transform.GetChild(0).transform.position;
        projectilePosition1 = this.gameObject.transform.GetChild(1).transform.position;
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
    public void Shoot(Vector3 directions)
    {
        float z = directions.z >= 270.0f ? directions.z - 360.0f : directions.z;
        //Vector3 newPosition = this.transform.position + new Vector3(projectilePosition.x * directions.x, projectilePosition.y * directions.y, 0.0f);
        Start2();
        if (itemValues.GetProjectile(atkType) != null)
        {
            //Debug.Log(z.ToString() + " " + Mathf.Abs(Mathf.Cos(z)).ToString() + " " + Mathf.Abs(Mathf.Sin(z)).ToString());
            set = false;
            GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType), projectilePosition, Quaternion.identity); //new Quaternion());// add position
            bullet.GetComponent<Projectile>().ignore = GameObject.Find(masterName);
            bullet.transform.right = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - projectilePosition1.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - projectilePosition1.y);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right*projectileVelocity;
            //bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileVelocity.x, 0.0f);//projectileVelocity.x*directions.x*Mathf.Abs(Mathf.Cos(z)), projectileVelocity.y * directions.y * Mathf.Abs(Mathf.Sin(z)));// make it normal
            //set any gun buffs for bullet.GetComponent<Projectile>().debuff
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.transform.parent == null)|| (this.transform.parent.name == "Items"))
        {
            if (collision.gameObject.GetComponent<PlayerControls>() != null)
            {
                collision.gameObject.GetComponent<PlayerControls>().IncludePickable(this.gameObject);
            }
        }
        else
        {
            if (set)
            {
                if (collision.gameObject.name.Contains(masterName))
                { return; }
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
