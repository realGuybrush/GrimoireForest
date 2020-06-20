using UnityEngine;

public class LandCheck : MonoBehaviour
{
    public BoxCollider2D landChecker;

    private bool landed;

    //this script requires a specified land trigger
    private void OnTriggerEnter2D(Collider2D c)
    {
        landed = true;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        landed = false;
    }

    public bool FirstJumpSuccessfull()
    {
        return !landed;
    }
}