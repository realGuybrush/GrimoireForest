using UnityEngine;

public class EnemyMovement : BasicMovement
{
    private Vector3 destination;

    private void Start()
    {
        destination = gameObject.transform.position;
        thisHealth = this.gameObject.GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        BasicCheckHealth();
    }

    public bool Near(Vector3 v)
    {
        var x = gameObject.transform.position.x;
        var y = gameObject.transform.position.y;
        if (destination.x > x - 0.01f && destination.x < x + 0.01f && destination.y > y - 0.01f &&
            destination.y < y + 0.01f)
        {
            return true;
        }

        return false;
    }

    private void SetDestination()
    {
        if (Near(destination))
        {
            if (CanSee())
            {
                //set player as target
            }
        }
        else
        {
            if (CanSee())
            {
                //set player as target
            }
            else
            {
                Follow();
            }
        }
    }

    private void Follow()
    {
        //follow target
        //if not following player
        // if there is floor
        //  go closer
        // else
        //  set target as this position
        //else
        // if there is floor
        //  go closer
        // else
        //  jump
    }

    private bool CanSee()
    {
        //if casted sector hit player
        return false;
    }
}
