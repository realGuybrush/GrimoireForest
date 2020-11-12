using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : EnemyMovement
{
    public Talk talk;
    public bool friendly = true;
    private void Start()
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
        talk.Load();
    }
    private void Update()
    {
        BasicCheckMidAir();
        if (!friendly)
        {
            if (attachedTo == -1)
            {
                FcknActAlready();
            }
            else
            {
                transform.localPosition = attPos;
                if (CanSee())
                {
                    SetDestination();
                    if (Near(destination))
                    {
                        Detach();
                        Attack();
                    }
                }
            }
            //this.transform.eulerAngles = new Vector3(0.0f, this.transform.eulerAngles.y, 0.0f);
            //anim.SetVar("MidAir", land.landed ? false : true);
        }
    }
}
