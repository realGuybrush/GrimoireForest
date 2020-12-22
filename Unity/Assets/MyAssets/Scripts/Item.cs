using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 projectilePosition;
    public Vector3 projectilePosition1;
    public Vector3 positionOnHand = new Vector3(1.630001f, -0.06000054f, 0.0f);
    public float projectileVelocity = 50.0f;
    public ItemCharacteristics itemValues = new ItemCharacteristics();
    public Sprite InventoryImage;
    private bool set;
    private int atkType;
    private string masterName;
    public int projectileIndex;
    public int projectilePerShot;
    public float atkSpd = 1.0f; //only on main atk for guns, on both atk for others

    public float strPenalty = 1.0f;
    public int strReq = 5;
    public int intReq = 5;

    public Events eve;

    //onCollision
    // search for hp script and extract hp.
    void Start()
    {
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
        masterName = "Player";
    }
    public void Attack(bool value, int type)
    {
        set = value;
        atkType = type;
    }
    public void Shoot()
    {
        if (itemValues.GetProjectile(atkType) != null)
        {
            set = false;
            if (this.gameObject.transform.childCount > 1)
            {
                projectilePosition = this.gameObject.transform.GetChild(0).transform.position;
                projectilePosition1 = this.gameObject.transform.GetChild(1).transform.position;
            }
            GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType), projectilePosition, Quaternion.identity, GameObject.Find("Projectiles").transform); //new Quaternion());// add position
            bullet.GetComponent<Projectile>().ignore = GameObject.Find(masterName);
            bullet.transform.right = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - projectilePosition1.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - projectilePosition1.y);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right*projectileVelocity;
        }
    }

    public void SetPenalty(float p)
    {
        strPenalty = p;
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
                if (Forbidden())
                { return; }
                if (collision.gameObject.GetComponent<Health>() != null)
                {
                    collision.gameObject.GetComponent<Health>().Substract((int)((this.transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayerControls>().ArmsStrength.value + itemValues.GetBuff(atkType).atk) * strPenalty));
                }
                if (collision.gameObject.GetComponent<BasicMovement>() != null)
                {
                    collision.gameObject.GetComponent<BasicMovement>().thisHealth.Substract((int)((this.transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayerControls>().ArmsStrength.value + itemValues.GetBuff(atkType).atk)*strPenalty));
                }
            }
        }
    }
    private bool Forbidden()
    {
        switch (atkType)
        {
            case 1:
                if ((itemValues.atk1 == "Atk2") || (itemValues.atk1 == "Atk5"))
                    return true;
                break;
            case 2:
                if ((itemValues.atk2 == "Atk2") || (itemValues.atk2 == "Atk5"))
                    return true;
                break;
            case 3:
                if ((itemValues.kick == "Atk2") || (itemValues.kick == "Atk5"))
                    return true;
                break;
            default:
                break;
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((this.transform.parent == null)|| this.transform.parent.name == "Items")
        {
            if (collision.gameObject.GetComponent<PlayerControls>() != null)
            {
                collision.gameObject.GetComponent<PlayerControls>().ExcludePickable(this.gameObject);
            }
        }
    }
}
