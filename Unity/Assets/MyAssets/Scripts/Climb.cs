using UnityEngine;

public class BasicClimb
{
    private readonly float climbXChange = 0.5f;//1.5f;
    private readonly float climbYChange = 0.0f;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newThisObject)
    {
        thisObject = newThisObject;
    }

    public bool CheckClimb(bool canClimb = true)
    {
        if (canClimb)
        {
            Climb();
            return true;
        }

        return false;
    }

    public void Climb()
    {
        var newPositionX = thisObject.transform.position.x + climbXChange * -thisObject.transform.forward.z;
        var newPositionY = thisObject.transform.position.y + climbYChange;
        var newPosition = new Vector2(newPositionX, newPositionY);
        thisObject.transform.position = newPosition;
        //var climbVector = new Vector2(climbXChange * thisObject.transform.forward.z, climbYChange);
        //thisObject.velocity = climbVector;
    }
}
