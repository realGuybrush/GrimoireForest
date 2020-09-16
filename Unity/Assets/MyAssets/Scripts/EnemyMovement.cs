using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : BasicMovement
{
    private Vector3 destination;
    Vector2 lookDirection = new Vector3(-1.0f, 1.0f);
    int minLookAngle = -30;
    int maxlookAngle = 45;
    float lookDistance = 3.0f;
    public float nearby = 1.5f;
    public bool walker = true;
    public bool jumper = false;
    public bool flyer = false;

    private void Start()
    {
        anim.a = gameObject.GetComponent<Animator>();
        destination = gameObject.transform.position;
        thisHealth = this.gameObject.GetComponent<Health>();
        move.SetThisObject(gameObject.GetComponent<Rigidbody2D>());
        flip.SetThisObject(gameObject.GetComponent<Rigidbody2D>());
        flip.facingRight = false;
    }

    private void FixedUpdate()
    {
        BasicCheckHealth();
    }

    private void Update()
    {
        FcknActAlready();
    }

    void FcknActAlready()
    {
        if (CanSee())
            SetDestination();
        if (!Near(destination))
        {
            SetDestination();
            Follow();
            UnAttack();
        }
        else
        {
            Stop();
            if (CanSee())
            {
                Attack();
            }
            else
            {
                UnAttack();
                if (Random.Range(0, 10) == 0)
                {
                    SetRandomDestination();
                }
            }
        }
    }

    public bool Near(Vector3 v)
    {
        //var x = gameObject.transform.position.x;
        //var y = gameObject.transform.position.y;
        //if (destination.x > x - 2.01f && destination.x < x + 2.01f && destination.y > y - 2.01f &&
        //    destination.y < y + 5.01f)
        RaycastHit2D[] R = Physics2D.RaycastAll(ToV2(transform.position), destination-transform.position, lookDistance, LayerMask.GetMask("Default"));
        //Debug.DrawRay(transform.position, destination-transform.position, Color.red, 10.0f);
        float dis;
        int i;
        for (i = 0; i < R.Length; i++)
        {
            if (R[i].collider.gameObject.name == "Player")
            {
                break;
            }
        }
        if (i < R.Length)
        {
            dis = R[i].distance;
            if (dis < nearby)
            {
                return true;
            }
        }
        return false;
    }

    private void SetDestination()
    {
        destination = GameObject.Find("Player").transform.position;
    }

    private void SetRandomDestination()
    {
        destination = new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), transform.position.y, transform.position.z);
    }

    private void Attack()
    {
        attacking = true;
        anim.SetVar("Atk", true);
    }

    private void UnAttack()
    {
        attacking = false;
        anim.SetVar("Atk", false);
    }

    private void Follow()
    {
        if (walker)
            WalkFollow();
        if (jumper)
            JumpFollow();
        if (flyer)
            FlyFollow();
    }
    void Stop()
    {
        move.movementMultiplier = 0;
        move.SlowDown(1.0f);
    }

    void WalkFollow()
    {
        move.movingDirection = (transform.position.x > destination.x)?-1:1;
        flip.CheckFlip(move.movingDirection);

        if (IsThereFloor())
        {
            move.movementMultiplier = (transform.position.x > destination.x) ? -1 : 1;
            move.Move();
        }
        else
        {
            Stop();
        }
        //move.movementMultiplier = ;
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
    void JumpFollow()
    {
    }
    void FlyFollow()
    {
    }
    bool IsThereFloor()
    {
        //Debug.DrawRay(transform.position, new Vector2(lookDistance * lookDirection.x * flip.FacingDirection() *  Mathf.Cos(-45*Mathf.Deg2Rad), lookDistance * lookDirection.y * Mathf.Sin(-45 * Mathf.Deg2Rad)), Color.blue, 10.0f);
        if (Physics2D.Raycast(ToV2(transform.position), new Vector2(lookDirection.x * flip.FacingDirection() * Mathf.Cos(-30 * Mathf.Deg2Rad), lookDirection.y * Mathf.Sin(-30 * Mathf.Deg2Rad)), lookDistance, LayerMask.GetMask("Environment")).collider != null)
            return true;
        return false;
    }
    Vector2 ToV2(Vector3 v)
    {
        return new Vector3(v.x, v.y);
    }

    private bool CanSee()
    {
        RaycastHit2D[] All;
        //if casted sector hit player
        for (int i = minLookAngle; i < maxlookAngle; i++)
        {
            //Debug.DrawRay(transform.position, new Vector2(lookDistance * lookDirection.x * Mathf.Cos(i * Mathf.Deg2Rad), lookDistance * lookDirection.y * Mathf.Sin(i * Mathf.Deg2Rad)), Color.green, 10.0f);
            All = Physics2D.RaycastAll(ToV2(transform.position), new Vector2(lookDirection.x * flip.FacingDirection() * Mathf.Cos(i * Mathf.Deg2Rad), lookDirection.y*Mathf.Sin(i * Mathf.Deg2Rad)), lookDistance, LayerMask.GetMask("Default"));
            for (int j = 0; j < All.Length; j++)
            {
                if (All[j].transform.gameObject.name == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }
}
