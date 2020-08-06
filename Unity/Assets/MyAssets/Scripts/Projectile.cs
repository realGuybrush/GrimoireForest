using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int atk = 5;
    public Buff debuff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().Substract(atk);
            collision.gameObject.GetComponent<Health>().AddBuff(debuff);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<BasicMovement>() != null)
        {
            collision.gameObject.GetComponent<BasicMovement>().thisHealth.Substract(atk);
            collision.gameObject.GetComponent<BasicMovement>().thisHealth.AddBuff(debuff);
            Destroy(this.gameObject);
        }
    }
}
