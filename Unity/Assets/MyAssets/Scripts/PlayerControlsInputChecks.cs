using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    ControlKeys keys = new ControlKeys();
    private void CheckMovementDirection()
    {
        move.movingDirection = Input.GetAxisRaw("Horizontal");
    }

    private void CheckSpeedUp()
    {
        move.movementMultiplier = Input.GetAxis("Horizontal");
    }

    private void CheckJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            if (!BasicCheckHold() && !anim.a.GetBool("Climb"))
            {
                if (move.crawl.crawling)
                {
                    BasicCheckRoll(flip.facingRight ? 1.0f : -1.0f);
                }
                else
                {
                    BasicCheckJump();
                }
            }
        }
        else
        {
            anim.SetVar("Jump", false);
        }
    }

    private void CheckClimbInput()
    {
        //if((Input.GetAxisRaw("Vertical") == 1.0f))
        if (BasicCheckHold() && Input.GetButton("Jump"))
        {
            BasicCheckClimb();
        }
    }

    public void CheckRunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
        {
            if (!move.crawl.crawling)
            {
                anim.SetVar("Run", true);
                move.run.Run();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetVar("Run", false);
            move.run.UnRun();
        }
    }

    public void CheckInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowHideMenu(inMenu, false, !inMenu, false, false);
        }
    }

    public void CheckPickUpInput()
    {
        if (PickableItem.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < PickableItem.Count; i++)
                {
                    PickableItem[i].GetComponent<Item>().Start2();
                }
                PickUp(PickableItem);
            }
        }
    }

    public void CheckCrawlInput()
    {
        /*if(Input.GetKeyDown(KeyCode.C) || Input.GetKey(KeyCode.C))
		{
			if(!move.run.running)
			{
				anim.SetVar("Crawl", true);
				move.crawl.Crawl();
			}
		}
		if(Input.GetKeyUp(KeyCode.C))
		{
			anim.SetVar("Crawl", false);
			move.crawl.UnCrawl();
		}*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!move.run.running && crawlTimer == 0 && !BasicCheckHold())
            {
                if (!anim.a.GetBool("Crawl"))
                {
                    anim.SetVar("Crawl", true);
                    move.crawl.Crawl();
                }
                else
                {
                    anim.SetVar("Crawl", false);
                    move.crawl.UnCrawl();
                }
            }
            else
            {
                if (BasicCheckHold())
                {
                    ReleaseHolds();
                    anim.SetVar("Grab", false);
                }
            }
        }
    }
    public void CheckAtkInput()
    {
        if (Weapon != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                BasicAtk1(true, GetAttackType(1));
            }

            if (Input.GetButtonUp("Fire1"))
            {
                BasicAtk1(false, GetAttackType(1));
            }

            if (Input.GetButtonDown("Fire2"))
            {
                BasicAtk1(true, GetAttackType(2));
                //thisHealth.AddBuff(-1, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }

            if (Input.GetButtonUp("Fire2"))
            {
                BasicAtk1(false, GetAttackType(2));
            }
        }
        if (Weapon != null)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                BasicAtk1(true, GetAttackType(3));
                //thisHealth.AddBuff(-1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }

            if (Input.GetButtonUp("Fire3"))
            {
                BasicAtk1(false, GetAttackType(3));
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire3"))
            {
                BasicAtk1(true, "Atk4");
                //thisHealth.AddBuff(-1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }

            if (Input.GetButtonUp("Fire3"))
            {
                BasicAtk1(false, "Atk4");
            }
        }
    }

    public void CheckDirections()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            BasicSetUp(true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            BasicSetUp(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            BasicSetDownp(true);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            BasicSetDownp(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            BasicSetRight(true);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            BasicSetRight(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            BasicSetLeft(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            BasicSetLeft(false);
        }
    }
    void CheckNumberInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSlotNumber = 0;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponSlotNumber = 1;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponSlotNumber = 2;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponSlotNumber = 3;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponSlotNumber = 4;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //spellSlotNumber = 0;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //spellSlotNumber = 1;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //spellSlotNumber = 2;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //spellSlotNumber = 3;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //spellSlotNumber = 4;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
    }
    void CheckEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideMenu(inMenu, !inMenu);
        }
    }
}
