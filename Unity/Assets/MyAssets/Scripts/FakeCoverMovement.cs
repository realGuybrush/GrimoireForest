using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCoverMovement : EnemyMovement
{
    public bool hidden = true;
    public int wakeChance = 0;
    public int wakeTime = 100;
    int curWakeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (attachedTo != -1)
            attPos = this.transform.localPosition;
        anim.a = gameObject.GetComponent<Animator>();
        destination = gameObject.transform.position;
        thisHealth = this.gameObject.GetComponent<Health>();
        move.SetThisObject(gameObject.GetComponent<Rigidbody2D>());
        flip.SetThisObject(gameObject.GetComponent<Rigidbody2D>());
        flip.facingRight = false;
        jump.SetThisObject(thisObject);
        climb.SetThisObject(thisObject);
        land = new BasicLand(thisObject, jump, climb);
        jump.jumpSpeedY = jumpHeight;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        TryTrigger();
    }

    void TryTrigger()
    {
        if (hidden)
        {
            if (Random.Range(0, 99) >= (100 - wakeChance))
            {
                UnHide();
                Attack();
                curWakeTime = wakeTime;
            }
        }
        else
        {
            if (!CanSee())
            {
                if (attacking)
                {
                    UnAttack();
                }
                if (curWakeTime > 0)
                {
                    curWakeTime--;
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                Attack();
                curWakeTime = wakeTime;
            }
        }
    }

    public void Hide()
    {
        hidden = true;
        anim.SetVar("Hide", true);
    }

    public void UnHide()
    {
        hidden = false;
        anim.SetVar("Hide", false);
    }

    public void SetWake(int wake)
    {
        wakeChance = wake;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludeCover(this.gameObject);
            SetWake(10);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            if (!hidden)
            {
                collision.gameObject.GetComponent<PlayerControls>().ExcludeCover(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludeCover(this.gameObject);
        }
    }
}
