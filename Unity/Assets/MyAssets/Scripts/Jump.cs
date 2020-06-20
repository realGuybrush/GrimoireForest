using UnityEngine;

public class BasicJump
{
    public int jumpsAmount = 1;
    public float jumpSpeedX = 3.0f;
    public float jumpSpeedY = 10.0f;
    public int successfulJumps;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newThisObject)
    {
        thisObject = newThisObject;
    }

    public bool CheckJump(bool midAir = false, bool canJump = true, float movingMultiplierX = 0.0f)
    {
        if (successfulJumps > 0 && successfulJumps < jumpsAmount && midAir)
        {
            Jump(movingMultiplierX);
            return true;
        }

        if (successfulJumps == 0 && canJump)
        {
            Jump(movingMultiplierX);
            return true;
        }

        return false;
    }

    public void Jump(float localMovingDirection, float jumpSpeedX = 0.0f)
    {
        /*if(wallLanded && canWallJump)
        {
            dontMoveX += localMovingDirection*walkSpeed;
            UnholdWall();
        }*/
        var jumpVector = new Vector2(jumpSpeedX * localMovingDirection, jumpSpeedY);
        thisObject.velocity = jumpVector;
        successfulJumps++;
    }
}