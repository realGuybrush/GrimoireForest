using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteppedOnTile : MonoBehaviour
{
    //fix: is it even needed, or we can just make checks on entering doors, saves and loads?
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.name.Contains("Ground"))
        {
            GameObject.Find("WorldManager").GetComponent<WorldManagement>().CalculatePlayerCorridorOffset((int)this.gameObject.transform.position.x);
        }
    }

}
