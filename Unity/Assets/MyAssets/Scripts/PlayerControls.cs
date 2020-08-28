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
        munitions.maxAmount = 11;
        munitions.Start();
        pickableItem = new List<GameObject>();
        SetDefaultBoneAngles();
        //ShowHideMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckWeaponSpell();
        if(!inMenu&&!IsClimbing())
        {
            PlayerCheckMove();
            CheckPass();
            CheckLand();
            CheckJumpInput();
            wall.UpdateHold();
            ledge.UpdateHold();
            move.crawl.UpdateRoll(anim.a.GetBool("Roll"));
            CheckFlip();
            CheckClimbInput();
            CheckAtkInput();
            CheckPickUpInput();
            CheckChestInput();
            CheckNumberInput();
            BasicCheckMidAir();
            BasicCheckHold();
            FollowCursor();
        }
        CheckInventoryInput();
        CheckSpellInput();
        CheckEsc();
        //BasicCheckRoll();
        //CheckDirections();
        if (crawlTimer > 0)
            crawlTimer--;
        //if (Weapon != null)
        //{
        //Weapon.transform.position = Weapon.GetComponent<Item>().positionOnHand+this.transform.position;
        //}
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
            flip.CheckFlip(move.movingDirection);
        }
    }

    public void ShowHideMenu(bool hideall = true, bool menu = false, bool weaponInv = false, bool spellInv = false, bool tradeInv = false, bool chestInv = false)
    {
        MenusTrigger MT = Book.GetComponent<MenusTrigger>();
        if (!MT.IsMenuActive())
        {
            inMenu = !hideall && (menu || weaponInv || spellInv || tradeInv || chestInv);
            Arms.SetActive(!inMenu);
            Book.SetActive(inMenu);
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
                MT.SetSpell(inventory, spells);
            }
            if (tradeInv)
            {
                //MT.SetTrade(inventory, otherinventory);
            }
            if (chestInv)
            {
                MT.SetTrade(inventory, chest[chosenChestIndex].GetComponent<Chest>().inventory);
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
