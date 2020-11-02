using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ignore;
    public int lifeTime = 100;
    public int atk = 5;
    public Buff debuff;
    public int specialRod = -1;
    public List<Events> onCollisionEvents = new List<Events>(); 
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), GameObject.Find(ignore.name).GetComponent<Collider2D>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime--;
        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int i1=0;
        if (atk > 0)
        {
            if ((collision.gameObject.layer == 8) || (collision.gameObject.layer == 9) || (collision.gameObject.layer == 12) || collision.gameObject == ignore)
            { return; }
            for (int i = 0; i < onCollisionEvents.Count; i++)
            {
                if (onCollisionEvents[i].activatable)
                {
                    onCollisionEvents[i].stringParameter = ignore.name;
                    onCollisionEvents[i].coords = this.transform.position;
                    onCollisionEvents[i].DoEvent();
                }
            }
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                i1 = collision.gameObject.GetComponent<Health>().HealthAmount();
                collision.gameObject.GetComponent<Health>().Substract(atk);
                if (debuff != null)
                {
                    collision.gameObject.GetComponent<Health>().AddBuff(debuff);
                }
            }
            if (collision.gameObject.GetComponent<BasicMovement>() != null)
            {
                collision.gameObject.GetComponent<BasicMovement>().thisHealth.Substract(atk);
                collision.gameObject.GetComponent<BasicMovement>().thisHealth.AddBuff(debuff);
            }
            atk -= i1;
            if (atk <= 0)
                Destroy(this.gameObject);
        }
    }
}
