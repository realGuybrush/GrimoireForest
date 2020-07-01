using UnityEngine;

public class PlayerControls : BasicMovement
{
    public GameObject Gun;
    public GameObject Sword;
    public GameObject Spear;
    public GameObject Arrow;
    public GameObject Rod;

    public GameObject MagicArtefact;
    public GameObject Bow;

    public GameObject Weapon;
    public GameObject PickableItem = null;
    private int crawlTimer;

    public GameObject Hip;
    public GameObject Back;
    public GameObject leftArm;
    public GameObject leftHand;
    public GameObject rightArm;
    public GameObject rightHand;
    private float turnAngleLeft;
    private float turnAngleRight;


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
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerCheckMove();
        CheckLand();
        CheckJumpInput();
        wall.UpdateHold();
        ledge.UpdateHold();
        move.crawl.UpdateRoll(anim.a.GetBool("Roll"));
        CheckFlip();
        CheckClimbInput();
        CheckAtkInput();
        CheckPickUpInput();
        BasicCheckMidAir();
        BasicCheckHold();
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

    public void CheckPickUpInput()
    {
        if (PickableItem != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SetWeapon(PickableItem);
                PickableItem = null;
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

    public void SetWeapon(GameObject weapon)
    {
        Weapon = weapon;
        //Weapon.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        switch (Weapon.GetComponent<Item>().itemValues.type)
        {
            case "Gun":
                Weapon.transform.SetParent(Gun.transform);
                break;
            case "Sword":
                Weapon.transform.SetParent(Sword.transform);
                break;
            case "Spear":
                Weapon.transform.SetParent(Spear.transform);
                break;
            case "Arrow":
                Weapon.transform.SetParent(Arrow.transform);
                break;
            case "Rod":
                Weapon.transform.SetParent(Rod.transform);
                break;
            case "MagicArtefact":
                Weapon.transform.SetParent(MagicArtefact.transform);
                break;
            case "Bow":
                Weapon.transform.SetParent(Bow.transform);
                break;
            default:
                break;
        }
        //Weapon.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        //it doesn't work correctly without magic numbers; get to the bottom of this, and exclude magic number later
        Weapon.transform.localPosition = Weapon.GetComponent<Item>().positionOnHand;// + this.transform.position;// - new Vector3(0.0f, 1.9f, 0.0f);
        //Weapon.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        //float sumRotation = Hip.transform.localRotation.z + Back.transform.localRotation.z + leftArm.transform.localRotation.z + leftHand.transform.localRotation.z;
        Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
    }
    public string GetAttackType(int attackIndex)
    {
        switch (attackIndex)
        {
            case 1:
                return Weapon.GetComponent<Item>().itemValues.atk1;
            case 2:
                return Weapon.GetComponent<Item>().itemValues.atk2;
            case 3:
                return Weapon.GetComponent<Item>().itemValues.kick;
            default:
                break;
        }
        return "";
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

    //todo
    //Можно сделать предметы, 
    //собирание предметов, 
    //навешивание их на специально выделенные объекты на руках, 
    //баффы предметов на атаки, 
    //распределение того, какая атака вызывает какую анимацию, 
    //генерация снарядов, 
    //манекен для битья, 
    // атака, 
    // оборона, 
    // смерть, 
    // рэгдолл трупа монстров, или анимации, 
    // рэгдолл игрока при слишком большом уроне, 
    //инвентарь, 
    // меню инвентаря, 
    //меню, 
    //генерация карты, 
    // генерация уровней, 
    // меню карты, 
    //сохранение, 
    //загрузка, 
    //главное меню.
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
    //more than one scene
    //interscene movement
}
