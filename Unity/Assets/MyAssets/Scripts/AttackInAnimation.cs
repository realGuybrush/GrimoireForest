using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInAnimation : StateMachineBehaviour
{
    private GameObject aO;
    private bool a1;
    private bool a2;
    private bool a3;
    private bool a4;
    private bool a5;
    private bool a6;
    private bool a7;
    private void SetA(Animator a)
    {
        a1 = a.GetBool("Atk1");
        a2 = a.GetBool("Atk2");
        a3 = a.GetBool("Atk3");
        a4 = a.GetBool("Atk4");
        a5 = a.GetBool("Atk5");
        a6 = a.GetBool("Atk6");
        a7 = a.GetBool("Atk7");
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aO = animator.gameObject;
        SetA(animator);
        if (aO.GetComponent<PlayerControls>() != null)
        {
            if (aO.GetComponent<PlayerControls>().Weapon != null)
            {
                Item item = aO.GetComponent<PlayerControls>().Weapon.GetComponent<Item>();
                if (a2 || a3 || a6)
                {
                    if (aO.GetComponent<PlayerControls>().inventory.Remove(item.projectileIndex, item.projectilePerShot))
                    {
                        aO.GetComponent<PlayerControls>().attacking = true;
                        aO.GetComponent<PlayerControls>().StartFollowingCursor();
                        item.Shoot(new Vector3((aO.GetComponent<PlayerControls>().flip.facingRight ? 1.0f : -1.0f), aO.GetComponent<PlayerControls>().Back.transform.localEulerAngles.z < 180 ? 1.0f : -1.0f, aO.GetComponent<PlayerControls>().Back.transform.localEulerAngles.z));
                        aO.GetComponent<PlayerControls>().BackFire(aO.GetComponent<PlayerControls>().Weapon.GetComponent<Item>().strReq);
                    }
                }
                if (a5)
                {
                    if (aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber] != -1)
                    {
                        if ((item.itemValues.number == GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber]].GetComponent<Projectile>().specialRod) ||
                          GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber]].GetComponent<Projectile>().specialRod == -1)
                        {
                            aO.GetComponent<PlayerControls>().attacking = true;
                            aO.GetComponent<PlayerControls>().StartFollowingCursor(true);
                        }
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aO = animator.gameObject;
        if (aO.GetComponent<PlayerControls>() != null)
        {
            if (aO.GetComponent<PlayerControls>().Weapon != null)
            {
                if (a1 || a4 || a7)
                {
                    aO.GetComponent<PlayerControls>().StartFollowingCursor(true);
                    aO.GetComponent<PlayerControls>().attacking = false;
                    aO.GetComponent<PlayerControls>().Weapon.GetComponent<Item>().Attack(false, 1);
                }
                if (a5)
                {
                    aO.GetComponent<PlayerControls>().attacking = false;
                    Item item = aO.GetComponent<PlayerControls>().Weapon.GetComponent<Item>();
                    //item.Start2();
                    if (aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber] != -1)
                    {
                        if ((item.itemValues.number == GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber]].GetComponent<Projectile>().specialRod) ||
                          GameObject.Find("WorldManager").GetComponent<WorldManagement>().ItemPrefabs[aO.GetComponent<PlayerControls>().spells.Items[aO.GetComponent<PlayerControls>().spellSlotNumber]].GetComponent<Projectile>().specialRod == -1)
                        {
                            //aO.GetComponent<PlayerControls>().attacking = false;
                            aO.GetComponent<PlayerControls>().StartFollowingCursor(true);
                            item.Shoot(new Vector3((aO.GetComponent<PlayerControls>().flip.facingRight ? 1.0f : -1.0f), aO.GetComponent<PlayerControls>().Back.transform.localEulerAngles.z < 180 ? 1.0f : -1.0f, aO.GetComponent<PlayerControls>().Back.transform.localEulerAngles.z));
                            aO.GetComponent<PlayerControls>().BackFire(aO.GetComponent<PlayerControls>().Weapon.GetComponent<Item>().intReq);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
        SetA(animator);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
