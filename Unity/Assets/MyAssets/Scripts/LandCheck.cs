using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    public BoxCollider2D landChecker;

    private int landed=0;
    private List<int> ignoreLayer = new List<int>();

    //this script requires a specified land trigger
    private void Start()
    {
        ignoreLayer.Add(8);
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if(!ignoreLayer.Contains(c.gameObject.layer))
            landed++;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (!ignoreLayer.Contains(c.gameObject.layer))
            landed--;
    }

    public bool FirstJumpSuccessfull()
    {
        return landed==0;
    }
}
