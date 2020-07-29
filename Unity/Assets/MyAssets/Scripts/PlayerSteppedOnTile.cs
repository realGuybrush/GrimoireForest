using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteppedOnTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.name.Contains("Ground"))
        {
            GameObject.Find("WorldManager").GetComponent<WorldManagement>().CalculatePlayerCorridorOffset((int)this.gameObject.transform.position.x);
        }
    }

}
