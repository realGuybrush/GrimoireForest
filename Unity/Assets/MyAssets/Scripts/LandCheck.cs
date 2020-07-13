using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    public BoxCollider2D landChecker;

    private bool landed;
    private List<int> ignoreLayer = new List<int>();

    //this script requires a specified land trigger
    private void Start()
    {
        ignoreLayer.Add(8);
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if(!ignoreLayer.Contains(c.gameObject.layer))
            landed = true;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (!ignoreLayer.Contains(c.gameObject.layer))
            landed = false;
    }

    public bool FirstJumpSuccessfull()
    {
        return !landed;
    }
}
