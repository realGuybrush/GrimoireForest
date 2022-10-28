using UnityEngine;

public class BasicMove
{
    public BasicCrawl crawl = new BasicCrawl();
    public float movementMultiplier = 0.0f;

    public float movingDirection = 0.0f;
    public BasicRun run = new BasicRun();
    private Rigidbody2D thisObject;
    public float walkSpeed = 3.0f;
    public float baseWalkSpeed = 3.0f;

    public void SetThisObject(Rigidbody2D newObject)
    {
        thisObject = newObject;
        crawl.SetThisObject(thisObject);
    }

    public void Move()
    {
        var bonusMultiplier = 1.0f;
        if (run.running)
        {
            bonusMultiplier *= run.runMultiplier;
        }

        if (crawl.crawling)
        {
            bonusMultiplier *= crawl.crawlingMultiplier;
        }

        var walkVector = new Vector2(bonusMultiplier * walkSpeed * movementMultiplier, thisObject.velocity.y);
        thisObject.velocity = walkVector;
    }

    public void SlowDown(float slowingAmount = 0.01f, float dontSlowY = 0.0f)
    {
        if (thisObject.velocity.x != 0)
        {
            var counterDirection = -Mathf.Sign(thisObject.velocity.x);
            Vector2 walkVector;
            if (Dif(thisObject.velocity.x, walkSpeed * slowingAmount) < Mathf.Abs(walkSpeed * slowingAmount))
            {
                walkVector = new Vector2(-thisObject.velocity.x, dontSlowY);
            }
            else
            {
                walkVector = new Vector2(walkSpeed * slowingAmount * counterDirection, dontSlowY);
            }

            thisObject.velocity += walkVector;
        }
    }

    private float Dif(float a, float b)
    {
        return Mathf.Abs(a - b);
    }
}
