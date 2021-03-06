using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    private int crawlTimer;

    public GameObject Hip;
    public GameObject Back;
    public GameObject Head;
    public GameObject leftArm;
    public GameObject leftHand;
    public GameObject rightArm;
    public GameObject rightHand;
    private float turnAngleLeft;
    private float turnAngleRight;
    private bool inMenu = true;

    public GameObject Arms;
    public GameObject Book;
    public GameObject Talks;

    bool fallingFromPlatform = false;


    // Use this for initialization
    private void Start()
    {
        turnAngleLeft = Hip.transform.localEulerAngles.z + Back.transform.localEulerAngles.z + leftArm.transform.localEulerAngles.z + leftHand.transform.localEulerAngles.z;
        turnAngleRight = Hip.transform.localEulerAngles.z + Back.transform.localEulerAngles.z + rightArm.transform.localEulerAngles.z + rightHand.transform.localEulerAngles.z;
        thisObject = gameObject.GetComponent<Rigidbody2D>();
        thisHealth = thisObject.GetComponent<Health>();
        anim.a = GetComponent<Animator>();
        move.SetThisObject(thisObject);
        flip.SetThisObject(thisObject);
        jump.SetThisObject(thisObject);
        land = new BasicLand(thisObject, jump, climb);
        ledge = new BasicLand(thisObject, jump, climb, 100000, false, true);
        wall = new BasicLand(thisObject, jump, climb, 0, false);
        step = new BasicLand(thisObject, jump, climb, 0, false);
        climb.SetThisObject(thisObject);
        inventory.Start();
        spellInventory.Start();
        munitions.maxAmount = 11;
        munitions.Start();
        pickableItem = new List<GameObject>();
        SetDefaultBoneAngles();
        RecalcCharacteristics();
        //ShowHideMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckWeaponSpell();
        if(!inMenu&&!IsClimbing())
        {
            CheckLand();
            BasicCheckMidAir();
            if (!IsTalking())
            {
                PlayerCheckMove();
                CheckHide();
                CheckPass();
                if (!CheckFallingFromPlatform())
                {
                    CheckAllJumpInput();
                }
                wall.UpdateHold();
                ledge.UpdateHold();
                move.crawl.UpdateRoll(anim.a.GetBool("Roll"));
                CheckFlip();
                CheckAtkInput();
                CheckActionInput();
                CheckNumberInput();
                BasicCheckHold();
                FollowCursor();
            }
            else
            {
                StopAttacking();
            }
        }
        BasicCheckHealth();
        CheckInventoryInput();
        CheckSpellInput();
        CheckEsc();
        if (crawlTimer > 0)
            crawlTimer--;
    }

    private void PlayerCheckMove()
    {
        CheckMovementDirection();
        CheckSpeedUp();
        CheckRunInput();
        CheckCrawlInput();
        if (move.movingDirection != 0)
        {
            if (!BasicCheckHold())
            {
                move.Move();
            }
        }
        else
        {
            if (!land.landed)
            {
                move.SlowDown();
            }
            else
            {
                var stopInstantly = 1.0f;
                move.SlowDown(stopInstantly);
            }
        }

        if (thisObject.velocity.x != 0 || thisObject.velocity.y != 0)
        {
            anim.SetVar("Moving", true);
        }
        else
        {
            anim.SetVar("Moving", false);
        }
    }

    private void CheckFlip()
    {
        if (!BasicCheckHold())
        {
            if ((!attacking)&&!IsFollowing())
            {
                flip.CheckFlip(move.movingDirection);
                FlipParasites();
            }
        }
    }

    private void FlipParasites()
    {
        if (transform.GetChild(0).GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<BasicMovement>().flip.facingRight = !transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<BasicMovement>().flip.facingRight;
            }
        }
    }

    public IEnumerator FallFromPlatform()
    {
        fallingFromPlatform = true;
        Physics2D.IgnoreLayerCollision(0, 12, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(0, 12, false);
        fallingFromPlatform = false;
    }

    public bool CheckFallingFromPlatform()
    {
        if (fallingFromPlatform)
        {
            Physics2D.IgnoreLayerCollision(0, 12, true);
            return true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(0, 12, false);
            return false;
        }
    }

    public void ShowHideMenu(bool hideall = true, bool menu = false, bool weaponInv = false, bool spellInv = false, bool tradeInv = false, bool chestInv = false, bool talkInv = false)
    {
        MenusTrigger MT = Book.GetComponent<MenusTrigger>();
        if (!MT.IsMenuActive())
        {
            inMenu = !hideall && (menu || weaponInv || spellInv || tradeInv || chestInv);
            if (inMenu)
            {
                GameObject.Find("WorldManager").GetComponent<WorldManagement>().StopTime();
            }
            else
            {
                GameObject.Find("WorldManager").GetComponent<WorldManagement>().UnStopTime();
            }
            Arms.SetActive(!inMenu&&!talkInv);
            Book.SetActive(inMenu&&!talkInv);
            Talks.SetActive(!inMenu&& talkInv);
            MT.ShowMenu(menu);
            MT.ShowInv(weaponInv);
            MT.ShowSpell(spellInv);
            MT.ShowTrade(tradeInv||chestInv);
            if (weaponInv)
            {
                MT.SetInv(inventory, munitions);
            }
            if (spellInv)
            {
                MT.SetSpell(spellInventory, spells);
            }
            if (tradeInv)
            {
                //MT.SetTrade(inventory, otherinventory);
            }
            if (chestInv)
            {
                MT.SetTrade(inventory, chest[chosenChestIndex].GetComponent<Chest>().inventory);
            }
            if (talkInv)
            {
                Talks.GetComponent<TalkMovement>().SetTalk(talks.GetComponent<NPCBehaviour>().talk, talks.transform.position, this.transform.position);
            }
        }
        else
        {
            if (MT.EscapeMenu())
            {
                ShowHideMenu(true);
            }
        }
    }

    public void Res()
    {
        landChecker.Res();
        wallChecker.Res();
        stepChecker.Res();
        ledgeChecker.Res();
    }
    public void LoadData(SVector3 position, SVector3 rotation, SVector3 speed, Characteristics newCharacteristics, Inventory newInventory, Inventory newMunitions, Inventory newSpells)
    {
        BasicLoadData(position, rotation, speed, newCharacteristics, newInventory);
        munitions = newMunitions;
        spells = newSpells;
    }
    //todo
    //Можно сделать предметы, 
    //собирание предметов, +
    //навешивание их на специально выделенные объекты на руках, + 
    //баффы предметов на атаки, +
    //распределение того, какая атака вызывает какую анимацию, +
    //генерация снарядов, +
    //манекен для битья, +
    // атака, +
    // оборона, +
    // смерть, +
    // рэгдолл трупа монстров, или анимации, 
    // рэгдолл игрока при слишком большом уроне, 
    //инвентарь, +
    // меню инвентаря, +
    //меню, +
    //генерация карты, +
    // генерация уровней, +
    // меню карты, 
    //сохранение, 
    //загрузка, 
    //главное меню. +
    //Потом враги, 
    // их ИИ, 
    // анимации, 
    //потом, - мультиплеер.
    //some sort of list of keys (in addition to Input Buttons)
    //animation and animation script +
    //weapon as part of animation +
    //weapon script
    //roll +
    //combinations of buttons x
    //more than one scene +
    //interscene movement +
}
