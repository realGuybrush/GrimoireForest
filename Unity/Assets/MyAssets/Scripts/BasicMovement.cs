using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Animations anim = new Animations();
    public BasicClimb climb = new BasicClimb();
    public BasicFlip flip = new BasicFlip();
    public BasicJump jump = new BasicJump();
    public BasicLand land;

    public LandCheck landChecker;
    public BasicLand ledge;
    public LandCheck ledgeChecker;

    public BasicMove move = new BasicMove();
    public BasicLand step;
    public LandCheck stepChecker;
    public Health thisHealth;

    public Rigidbody2D thisObject;
    public BasicLand wall;
    public LandCheck wallChecker;

    public bool attacking = false;
    public Inventory inventory = new Inventory();

    public void BasicCheckHealth()
    {
        if (thisHealth.values.GetHealth() <= 0)
        {
            DropItems();
            Destroy(gameObject);//replace it with body dropping later on
        }
    }
    public void DropItems()
    {
        WorldManagement WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        GameObject d = GameObject.Instantiate(WM.DropPrefab, this.transform.position, this.transform.rotation);
        inventory.Items[0] = 0;//for show
        inventory.stacks[0] = 1;//uncomment this
        d.GetComponent<Chest>().SetInventory(inventory);
    }
    public bool BasicCheckHold(bool setGrabValue = false)
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        if (holding)// setGrabValue
        {
            anim.SetVar("Grab", holding);
        }
        else
        {
            anim.SetVar("Grab", holding);
        }
        return holding;
    }

    public void CheckLand()
    {
        var touchingLand = !landChecker.FirstJumpSuccessfull();
        var touchingTop = !ledgeChecker.FirstJumpSuccessfull();
        var touchingMid = !wallChecker.FirstJumpSuccessfull();
        var touchingBot = !stepChecker.FirstJumpSuccessfull();
        var onHold = wall.holding || ledge.holding;
        if (!onHold)
        {
            var pressingTowardsWall = move.movingDirection < 0 && !flip.facingRight ||
                                      move.movingDirection > 0 && flip.facingRight;
            land.CheckThisLand(touchingLand);
            if (!land.landed && pressingTowardsWall) // && !thisHealth.values.attacking)
            {
                ledge.CheckThisLand(!touchingTop && touchingMid);
                wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
                step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
            }
            else
            {
                ledge.landed = false;
                wall.landed = false;
                step.landed = false;
            }
        }
        else
        {
            ledge.CheckThisLand(!touchingTop && touchingMid);
            wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
            step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
        }

        //ledge.CheckThisLand(!touchingTop && touchingMid);
    }

    public void BasicCheckJump(float movingMultiplierX = 0.0f)
    {
        var midAir = !(land.landed || ledge.landed || wall.landed || step.landed);
        var canJumpFromCurrentLand = land.landed && land.canJump || ledge.landed && ledge.canJump ||
                                     wall.landed && wall.canJump || step.landed && step.canJump;
        if (jump.CheckJump(midAir, canJumpFromCurrentLand && !move.crawl.crawling, movingMultiplierX))
        {
            anim.SetVar("Jump", true);
            ReleaseHolds();
        }
    }

    public void BasicCheckRoll(float movingMultiplierY = 0.0f)
    {
        if (move.crawl.CheckRoll(move.crawl.crawling, movingMultiplierY))
        {
            anim.SetVar("Jump", true);
            anim.SetVar("Roll", true);
        }
    }

    public bool IsClimbing()
    {
        return anim.a.GetBool("Climb");
    }

    public void BasicCheckClimb()
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        var canClimb = land.holding && land.canClimb || ledge.holding && ledge.canClimb ||
                       wall.holding && wall.canClimb || step.holding && step.canClimb;
        if (holding)
        {
            if (climb.CheckClimb(canClimb))
            {
                anim.SetVar("Climb", true);
                ReleaseHolds();
            }
            else
            {
                anim.SetVar("Climb", false);
            }
        }
        else
        {
            anim.SetVar("Climb", false);
        }
    }

    public void BasicAtk1(bool atk, string attackType, Buff buff)
    {
        anim.SetVar(attackType, atk);
        thisHealth.values.attacking = true;
        thisHealth.values.AddBuff(buff);
    }

    /*public void BasicAtk2(bool atk)
    {
        anim.SetVar("Atk2", atk);
        thisHealth.values.attacking = true;
    }

    public void BasicAtk3(bool atk)
    {
        anim.SetVar("Atk3", atk);
        thisHealth.values.attacking = true;
    }*/

    public void BasicCheckMidAir()
    {
        if (landChecker.FirstJumpSuccessfull())
        {
            anim.SetVar("MidAir", true);
        }
        else
        {
            anim.SetVar("MidAir", false);
        }
    }

    public void BasicSetUp(bool value)
    {
        anim.SetVar("Up", value);
    }

    public void BasicSetDownp(bool value)
    {
        anim.SetVar("Down", value);
    }

    public void BasicSetRight(bool value)
    {
        anim.SetVar("Right", value);
    }

    public void BasicSetLeft(bool value)
    {
        anim.SetVar("Left", value);
    }

    public void ReleaseHolds()
    {
        land.Unhold();
        ledge.Unhold();
        wall.Unhold();
        step.Unhold();
        //anim.SetVar("Grab", false);
    }
}
