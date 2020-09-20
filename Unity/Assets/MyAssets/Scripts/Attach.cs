using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Player")
        {
            this.transform.parent = c.transform.GetChild(0).GetChild(0);
            this.transform.gameObject.GetComponent<EnemyMovement>().anim.SetVar("Grab", true);
            this.transform.gameObject.GetComponent<EnemyMovement>().attPos = new Vector3(0.55f, -0.25f, 0.0f);//not 0, 0; trust me, I'm an engineer
            this.transform.gameObject.GetComponent<EnemyMovement>().attachedTo = -2;
        }
    }
}
