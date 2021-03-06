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

    private void CheckPass()
    {
        if (land.landed)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (HaveSuchPassage(DirectionType.North))
                    Pass(DirectionType.North);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (HaveSuchPassage(DirectionType.South))
                    Pass(DirectionType.South);
            }
            if (move.movementMultiplier != 0)
            {
                if (move.movingDirection == -1)
                {
                    if (HaveSuchPassage(DirectionType.West))
                        Pass(DirectionType.West);
                }
                else
                {
                    if (HaveSuchPassage(DirectionType.East))
                        Pass(DirectionType.East);
                }
            }
        }
    }
    public void CheckAllJumpInput()
    {
        if(!CheckFallPlatformInput())
            if(!CheckClimbInput())
                CheckJumpInput();
    }
    private bool CheckJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            if (!BasicCheckHold() && !IsClimbing())
            {
                if (move.crawl.crawling)
                {
                    BasicCheckRoll(flip.facingRight ? 1.0f : -1.0f);
                }
                else
                {
                    BasicCheckJump();
                }
                return true;
            }
        }
        else
        {
            anim.SetVar("Jump", false);
        }
        if (Input.GetButtonUp("Jump"))
        {
            jump.StopJump(move.movingDirection);
        }
        return false;
    }

    private bool CheckClimbInput()
    {
        if (BasicCheckHold() && Input.GetButton("Jump"))
        {
            BasicCheckClimb();
            return true;
        }
        return false;
    }

    public bool CheckFallPlatformInput()
    {
        if (Input.GetButtonDown("Jump") && (Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow)))
        {
            bool b = Physics2D.GetIgnoreLayerCollision(0, 12);
            StartCoroutine("FallFromPlatform");
            return true;
        }
        return false;
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
            DeleteEmptyDrops();
            ShowHideMenu(inMenu, false, !inMenu, false, false, false);
        }
    }
    public void CheckSpellInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowHideMenu(inMenu, false,  false, !inMenu, false, false);
        }
    }

    public void CheckActionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!CheckPickUpInput())
                if (!CheckChestInput())
                    if(!BasicCheckMidAir())
                        CheckTalkInput();
        }
    }

    public bool CheckTalkInput()
    {
        if (talks != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartTalk();
                return true;
            }
        }
        return false;
    }

    public bool CheckPickUpInput()
    {
        if (pickableItem.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp(pickableItem);
                return true;
            }
        }
        return false;
    }
    public bool CheckChestInput()
    {
        if ((pickableItem.Count == 0)&&(chest.Count != 0))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!inMenu)
                {
                    ChooseChest();
                    ShowHideMenu(inMenu, false, false, false, false, !inMenu);
                }
                else
                {
                    DeleteEmptyDrops();
                    ShowHideMenu(inMenu, false, false, false, false, !inMenu);
                }
                return true;
            }
        }
        return false;
    }

    public void CheckCrawlInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!hidden)
            {
                if (!move.run.running && crawlTimer == 0 && !BasicCheckHold() && !CanHide())
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
                    if (CanHide())
                    {
                        move.crawl.Crawl();
                        Hide();
                    }
                }
            }
            else
            {
                move.crawl.UnCrawl();
                UnHide();
            }
        }
    }
    public void CheckAtkInput()
    {
        if (Weapon != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if ((GetAttackType(1) == "Atk5")&&(spells.Items[spellSlotNumber] != -1)||(GetAttackType(1)!="Atk5"))
                {
                    BasicAtk1(true, GetAttackType(1), GetBuff(1));
                    Weapon.GetComponent<Item>().Attack(true, 1);
                    RecalcAtkSpd(GetAttackType(1));
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                BasicAtk1(false, GetAttackType(1), GetBuff(1));
            }

            if (Input.GetButtonDown("Fire2"))
            {
                BasicAtk1(true, GetAttackType(2), GetBuff(2));
                Weapon.GetComponent<Item>().Attack(true, 2);
                RecalcAtkSpd(GetAttackType(1)=="Atk2"?"":GetAttackType(2));
            }

            if (Input.GetButtonUp("Fire2"))
            {
                BasicAtk1(false, GetAttackType(2), GetBuff(2));
            }
        }
        if (Weapon != null)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                BasicAtk1(true, GetAttackType(3), GetBuff(3));
                Weapon.GetComponent<Item>().Attack(true, 3);
                RecalcAtkSpd(GetAttackType(1) == "Atk2" ? "" : GetAttackType(3));
            }

            if (Input.GetButtonUp("Fire3"))
            {
                BasicAtk1(false, GetAttackType(3), GetBuff(3));
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire3"))
            {
                BasicAtk1(true, "Atk4", new Buff());
            }

            if (Input.GetButtonUp("Fire3"))
            {
                BasicAtk1(false, "Atk4", new Buff());
            }
        }
    }

    public void StopAttacking()
    {
        if (Weapon != null)
        {
            BasicAtk1(false, GetAttackType(1), GetBuff(1));
            BasicAtk1(false, GetAttackType(2), GetBuff(2));
            BasicAtk1(false, GetAttackType(3), GetBuff(3));
        }
        anim.SetVar("Moving", false);
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
            spellSlotNumber = 0;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            spellSlotNumber = 1;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            spellSlotNumber = 2;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            spellSlotNumber = 3;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            spellSlotNumber = 4;
            CheckWeaponSpell();
            Arms.GetComponent<ArmsDepiction>().UpdateWSNumber();
        }
    }
    void CheckEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideMenu(inMenu, !inMenu);
            DeleteEmptyDrops();
            Talks.GetComponent<TalkMovement>().Exit();
        }
    }
}
