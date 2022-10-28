using UnityEngine;

public class BasicFlip
{
    public bool facingRight = true;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newObject)
    {
        thisObject = newObject;
    }

    public void CheckFlip(float movingDirection)
    {
        if (facingRight && movingDirection < 0)
        {
            Flip();
        }
        else if (!facingRight && movingDirection > 0)
        {
            Flip();
        }
    }
    public void CheckFlip()
    {
        if (facingRight && (FacingDirection() > 0))
        {
            Flip();
        }
        else if (!facingRight && (FacingDirection() < 0))
        {
            Flip();
        }
    }

    public void Flip()
    {
        var rotateAroundY = new Vector3(0.0f, 180.0f, 0.0f);
        thisObject.transform.Rotate(rotateAroundY);
        facingRight = !facingRight;
    }

    public float FacingDirection()
    {
        if (thisObject.name == "Player")
            return (thisObject.transform.eulerAngles.y == 0.0f ? 1.0f : -1.0f);
        else
            return (thisObject.transform.eulerAngles.y == 0.0f ? -1.0f : 1.0f);
    }
}
