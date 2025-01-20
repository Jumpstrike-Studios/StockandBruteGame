using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Actor_Player : Actor
{
   
    public GameObject Stock_Sprite;
    public GameObject Brute_Sprite;
    public GameObject StockHealthBar;
    public GameObject BruteHealthBar;
    public GameObject PunchBox;

    public ActorVitals BruteHealth;
    public ActorVitals StockHealth;

    public float absDashCooldown = 0.5f;
    public float acceleration;
    public float deceleration;
    public float JumpPower = 3.5f;


    private float accelerationTime;
    private float curDashCooldown = 0;
    private float DashTimer;
    private float TrailTimer;
    private float PunchBoxTimer;

    public bool Can_DoubleJump;
    private bool Can_ExtraJump {get{return IsStock && 
    (CollisionState== Collision_State.Airborne || CollisionState== Collision_State.AgainstWall) 
    && (Can_DoubleJump || Is_EligibleForWallAction); }}

    public bool IsStock=false;
    private bool HasDashed;

    private float UIChange;
    private int LastChange;
    
    private Vector2 DashDirection;


    public float IFrames;
    public int IFrame_Ticker;
    public void UpdateDamage()
    {
        if (this.IFrames > 0)
            IFrames -= Time.deltaTime;
        if (IFrames <= 0 && IFrame_Ticker >= 0)
        {
            IFrame_Ticker--;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        Debug.Log("Player has taken damage");
        Health.Health -= damage;
        IFrames = 1 / 60f;
        IFrame_Ticker = 40;
        // break the wall
        if (Health.Health <= 0)
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Enemy") && IFrame_Ticker <= 0)
        {
            TakeDamage(100);
        }
    }

    new public void Start()
{
base.Start();
 StockHealth = new ActorVitals(200);
 BruteHealth = new ActorVitals(500);
 Health = BruteHealth;
 StockHealth.RemoveOnDeath=false;
 BruteHealth.RemoveOnDeath=false;
 OnStateChange += OnStateChange_Player;
}

public void UpdateTrail(){
    TrailTimer-=Time.deltaTime;
    if(TrailTimer < 0){
       GameObject trail = Instantiate(AfterImage,IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().transform.position:Brute_Sprite.GetComponent<SpriteRenderer>().transform.position,transform.rotation);
        trail.GetComponent<SpriteRenderer>().sprite = IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().sprite:Brute_Sprite.GetComponent<SpriteRenderer>().sprite;
        trail.GetComponent<SpriteRenderer>().flipX = IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().flipX:Brute_Sprite.GetComponent<SpriteRenderer>().flipX;
        TrailTimer=0.09f;
    }
}

public Animator GetAnimator(){return (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<Animator>();}

float easeInOutCubic( float x){
return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;}






    void UpdateUI()
{
    Vector3 AnchorPoint = new Vector3(-670, 340, 0);
    float TrigTimer = (easeInOutCubic(UIChange)-0.25f+LastChange) * Mathf.PI;
    Vector3 FinalPosition(float Offset)
    {
        return new Vector3(Mathf.Sin(TrigTimer + Offset * Mathf.PI), Mathf.Cos(TrigTimer + Offset * Mathf.PI)*0.5f, 0)*120 + AnchorPoint;
    }
    float SawTooth = Mathf.Asin(Mathf.Cos((LastChange+UIChange)*Mathf.PI+Mathf.PI))/2+0.5f;
    StockHealthBar.transform.localPosition = FinalPosition(1);
    BruteHealthBar.transform.localPosition = FinalPosition(0);
    StockHealthBar.transform.SetSiblingIndex(SawTooth>0.5?0:1);
    StockHealthBar.transform.SetSiblingIndex(SawTooth>0.5?1:0);
    StockHealthBar.transform.localScale = Vector3.Lerp(Vector3.one/2f,Vector3.one,easeInOutCubic(SawTooth));
    BruteHealthBar.transform.localScale = Vector3.Lerp(Vector3.one,Vector3.one/2f,easeInOutCubic(SawTooth));

    if(UIChange>=1f)
    {
        LastChange+=1;
        UIChange=0;
    }
}

// Update is called once per frame
    new void Update()
    {
        base.Update();
        UpdateDamage();
        float dt = Time.deltaTime;
        curDashCooldown -= dt;

        if (HasDashed && curDashCooldown < 0 && Is_OnGround)
        {
            HasDashed = false;
            DashTimer = 0;
        }

        UpdateUI();

        UIChange=Mathf.Clamp(UIChange+Time.deltaTime*(UIChange>0f?1:0), 0f,1f);
        //Change character sprite
        if (Input.GetKeyDown("q") && DashTimer<=0 && PunchBoxTimer<=0&&UIChange<=0f)
        {
            UIChange+=Time.deltaTime;
            if (!IsStock)
            {
                
                // Changes sprite to stock
                Stock_Sprite.SetActive(true); 
                Brute_Sprite.SetActive(false);
                
                JumpPower = 7.0f;
                Health = StockHealth;
                IsStock = true;

               

            }
            else
            {

                // Changes sprite to brute
                Stock_Sprite.SetActive(false);
                Brute_Sprite.SetActive(true);
                JumpPower = 4.5f;
                Health = BruteHealth;
                IsStock = false;

                
               
            }
        }

        //Fight or Flight
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (IsStock && !HasDashed && DashTimer <= 0)
            {

                DashDirection = new Vector2(Input.GetKey("a")?-1f:Input.GetKey("d")?1f:0,Input.GetKey("s")?-1f:Input.GetKey("w")?1f:0);
                Stock_Sprite.GetComponent<SpriteRenderer>().flipX = Input.GetKey("a");
                //Stock_Sprite.GetComponent<SpriteRenderer>().flipY = Input.GetKey("s");
                if (DashDirection.magnitude==0) DashDirection = Vector2.left*(Stock_Sprite.GetComponent<SpriteRenderer>().flipX?1:-1);
                DashTimer = 0.5f;
                HasDashed = true;
                curDashCooldown = absDashCooldown;
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && !IsStock && PunchBoxTimer <= 0)
        {
                if(Is_OnGround)
                {
                    GetAnimator().SetBool("Attacking",true);
                    PunchBoxTimer=0.8f;
                    PunchBox.transform.localPosition = new Vector3(1.026f*(Brute_Sprite.GetComponent<SpriteRenderer>().flipX?-1:1),0.202f,0);
                }
                else{
                    DashTimer = 1f;
                    PunchBoxTimer=1f;
                    DashDirection = Vector2.down;
                    //Brute_Sprite.GetComponent<SpriteRenderer>().flipY = true;
                    PunchBox.transform.localPosition = new Vector3(0,-1f,0);
                }
        }

        PunchBox.SetActive(PunchBoxTimer>0&&PunchBoxTimer<0.4);
        
        if(PunchBoxTimer>0)
        {
            PunchBoxTimer-=Time.deltaTime;
            rb.velocity =new Vector2(0,rb.velocity.y);
            Brute_Sprite.GetComponent<SpriteRenderer>().color = (PunchBoxTimer>0&&PunchBoxTimer<0.4)?Color.red:Color.white;

            //transform.localScale = Vector2.one+new Vector2(Mathf.Abs(DashDirection.x),Mathf.Abs(DashDirection.y))*DashTimer*2;
            if(DashTimer>0)
            {
                UpdateTrail();
                DashTimer-=Time.deltaTime;
                rb.velocity = DashDirection*acceleration*math.clamp(DashTimer*4,0f,1f)*1.5f;
                rb.gravityScale=0;
                PunchBox.SetActive(PunchBoxTimer>0);
                Brute_Sprite.GetComponent<SpriteRenderer>().color = (PunchBoxTimer>0)?Color.red:Color.white;
            }
        }
        else if(DashTimer>0)
        {
            UpdateTrail();
            DashTimer-=Time.deltaTime;
            rb.velocity = DashDirection*acceleration*math.clamp(DashTimer*4,0f,1f)*1.5f;
            rb.gravityScale=0;
        }
        else
        {
            PunchBox.transform.localPosition = new Vector3(1.026f*(Brute_Sprite.GetComponent<SpriteRenderer>().flipX?-1:1)*-1f,0.202f,0);
            GetAnimator().SetBool("Attacking",false);
            rb.gravityScale=1;
            Stock_Sprite.GetComponent<SpriteRenderer>().flipY =false;
            Brute_Sprite.GetComponent<SpriteRenderer>().flipY =false;

            if (Input.GetKey("d"))
            {
                accelerationTime+= Time.deltaTime*acceleration/3f;
                Velocity.x += acceleration * Time.deltaTime+accelerationTime/5;
                Stock_Sprite.GetComponent<SpriteRenderer>().flipX = false;
                Brute_Sprite.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (Input.GetKey("a"))
            {
                accelerationTime+= Time.deltaTime*acceleration/3f;
                Velocity.x -= acceleration * Time.deltaTime+accelerationTime/5;
                Stock_Sprite.GetComponent<SpriteRenderer>().flipX = true;
                Brute_Sprite.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            if (!(Input.GetKey("a")||Input.GetKey("d"))||(Input.GetKey("a")&&Input.GetKey("d"))) 
            {
                accelerationTime-= Time.deltaTime*deceleration;
                Velocity.x -= deceleration * Mathf.Sign(Velocity.x) * Time.deltaTime;
            }

            GetAnimator().SetInteger("Air State",Is_OnGround?rb.velocity.y<0?2:0:DashTimer>0?3:rb.velocity.y<0?2:1);
            GetAnimator().SetBool("Moving", accelerationTime>0);
            accelerationTime = Mathf.Clamp(accelerationTime, 0f,1f);
            Velocity.x = Mathf.Clamp(Velocity.x, -1f, 1f);
            rb.velocity = new Vector2(Velocity.x * WalkSpeed*accelerationTime, rb.velocity.y);
        }
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (Is_OnGround || Can_ExtraJump) // Checks if player is grounded then if doubleJump is true
            {
                if(Can_ExtraJump && Is_EligibleForWallAction){ My_PreviousWall = Is_OnWhatWall;
                Velocity.x = (int)Is_OnWhatWall;
                Can_DoubleJump =true;
                }else
                if(Can_ExtraJump && Can_DoubleJump) Can_DoubleJump =false;
                rb.velocity = new Vector2(Velocity.x * WalkSpeed*accelerationTime,JumpPower);
            }
        }
        if (Input.GetKeyUp("space") && rb.velocity.y > 0f) // If space is let go mid-jump, upwards velocity halved for smaller jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void OnStateChange_Player(Collision_State From, Collision_State To){
        //Left the ground in some manor, either by jumping or walking off a
        Debug.Log(From.ToString()+">"+To.ToString());
        if(From == Collision_State.OnGround && To == Collision_State.Airborne){
            Can_DoubleJump =true;
        }
        if(From == Collision_State.Airborne){
            DashTimer=0;
            PunchBoxTimer=0;
            rb.velocity =new Vector2(0,rb.velocity.y);
            Brute_Sprite.GetComponent<SpriteRenderer>().color =Color.white;
            PunchBox.SetActive(false);
            }
        }
        
    }
    

