using UnityEngine;

public class BasicCrawl
{
    public bool crawling;
    public float crawlingMultiplier = 0.5f;
    public float rollSpeedX = 10.0f;
    public float rollSpeedY = 0.0f;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newThisObject)
    {
        thisObject = newThisObject;
    }

    public void Crawl()
    {
        crawling = true;
    }

    public void UnCrawl()
    {
        crawling = false;
    }

    public void UpdateRoll(bool canRoll = false, float movingMultiplierY = 0.0f)
    {
        if (canRoll)
        {
            Roll(movingMultiplierY);
        }
    }

    public bool CheckRoll(bool canRoll = true, float movingMultiplierY = 0.0f)
    {
        if (crawling && canRoll)
        {
            Roll(movingMultiplierY);
            return true;
        }

        return false;
    }

    public void Roll(float localMovingDirection, float rollSpeedY = 0.0f)
    {
        var rollVector = new Vector2(rollSpeedX * thisObject.transform.forward.z, rollSpeedY);
        thisObject.velocity = rollVector;
        //thisObject.AddForce(thisObject.transform.forward*rollSpeedX);
    }
}