using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random=UnityEngine.Random;
public class Actor_Player : Actor
{
    public bool IsStock = false;
    public bool IsBrute => !IsStock;
    public GameObject Stock_Sprite;
    public GameObject Brute_Sprite;
    public GameObject StockHealthBar;
    public GameObject BruteHealthBar;
    public GameObject PunchBox;

    public static ActorVitals BruteHealth;
    public static ActorVitals StockHealth;
    private Vector2 tempVelocity;
    //TODO: it should be the order of base > dash > slam 
    public Vector2 LocalVelocity { get{
        
        if (Math.Abs(tempVelocity.x)-Time.deltaTime>=0f)tempVelocity.x-=Time.deltaTime*WalkSpeed*Math.Sign(tempVelocity.x);
        else tempVelocity.x=0f;
        if (Math.Abs(tempVelocity.y)-Time.deltaTime>=0f)tempVelocity.y-=Time.deltaTime*WalkSpeed*Math.Sign(tempVelocity.y);
        else tempVelocity.y=0f;
        tempVelocity.x = math.clamp(tempVelocity.x,-WalkSpeed*3f,WalkSpeed*3f);
        tempVelocity.y = math.clamp(tempVelocity.y,-WalkSpeed*3f,WalkSpeed*3f);
        return Velocity*Vector2.right*WalkSpeed*accelerationTime+Vector2.up*rb.velocity.y+tempVelocity;
    }}

    public float acceleration;
    public float deceleration;
    public float accelerationTime;

    public float IFrames;
    public int IFrame_Ticker;
     private float TrailTimer;

    //stock's abilities
    //The stored state prior to the dash
    public struct StoredMomentumState{
        public Vector2 VelocityOutside;
        public Vector2 VelocityInside;
        public float Acceleration;
        public float AccelerationTime;
    }

    public StoredMomentumState OutofDashState = new();

    //The dash is such a frucking pain... send help.
    private bool DashEnabled => IsStock && DashCooldown<=0; 
    private Vector2 DashDirection;
    private float DashPower=0f;
    private float DashPowerVelocity;

    private bool DashActive;
    private bool DashUsedInAir;
    private float DashCooldown = 0;


    public bool Can_DoubleJump;
    bool Can_ExtraJump => IsStock && 
    (CollisionState== Collision_State.Airborne || CollisionState== Collision_State.AgainstWall) 
    && (Can_DoubleJump || Is_EligibleForWallAction);


    //Brute's abilities
    private float SlamForgiveness=0.1f;
    bool SlamEnabled => IsBrute && !PunchEnabled
    && Velocity.y <= SlamForgiveness;

    public float JumpPower = 3.5f;
    
    bool PunchEnabled => IsBrute && Is_OnGround;

    private bool PunchActive;
    private bool SlamActive;
    private bool SlamOrPunchActive => SlamActive || PunchActive;
    private float PunchTimer;
    private float PunchCooldown=0;

    public bool Duo_Dead => StockHealth.Health == 0 || BruteHealth.Health==0;
    private float Anim_death_time = 0;
   //duo

   bool CanSwap => !SlamOrPunchActive && !DashActive && UIChange == 0f;
    //ui

    private float UIChange;
    private int LastChange;

    //events
     public delegate void GameOver(bool ActuallyOver=false);
    public static event GameOver? OnGameOver;

    
    public void UpdateDamage()
    {
        if (this.IFrames > 0)
            IFrames -= Time.deltaTime;
        if (IFrames <= 0 && IFrame_Ticker >= 0)
        {
            IFrame_Ticker--;
        }
    }

    public override void takeDamage(int damage)
    {
        Debug.Log(damage);
        Debug.Log("Player has taken damage");
        Health.Health -= damage;
        IFrames = 1 / 60f;
        IFrame_Ticker = 100;
        // break the wall
        if (Health.Health <= 0)
        {
            
            Die();
           
        }
    }
    public override void Die()
    {
        
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && IFrame_Ticker <= 8 && IFrame_Ticker >0 &&  !Duo_Dead)
        {
        IFrame_Ticker=10;
        }
        if (col.gameObject.CompareTag("Enemy") && IFrame_Ticker <= 0 && !Duo_Dead)
        {
            takeDamage(100);
        }
    }

    new public void Start()
{
base.Start();
 StockHealth = new ActorVitals(300);
 BruteHealth = new ActorVitals(500);
 StockHealth.RemoveOnDeath=false;
 BruteHealth.RemoveOnDeath=false;

 Health = BruteHealth;

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

    for (int i = 0; i < StockHealthBar.transform.GetChild(0).transform.GetChild(0).childCount; i++)
    {
     GameObject HealthBip = StockHealthBar.transform.GetChild(0).transform.GetChild(0).GetChild(i).gameObject;
        HealthBip.SetActive(i<Math.Ceiling(StockHealth.Health/100f));
       
        switch(Math.Ceiling(StockHealth.Health/100f)){
            default:
            case 3:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State1);break;
            case 2:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State2);break;
            case 1:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State3);break;
        }
    }
    for (int i = 0; i < BruteHealthBar.transform.GetChild(0).transform.GetChild(0).childCount; i++)
    {
         GameObject HealthBip = BruteHealthBar.transform.GetChild(0).transform.GetChild(0).GetChild(i).gameObject;
        HealthBip.SetActive(i<Math.Ceiling(BruteHealth.Health/100f));
         switch(Math.Ceiling(BruteHealth.Health/100f)){
            default:
            case 5:
            case 4:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State1);break;
            case 3:
            case 2:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State2);break;
            case 1:  HealthBip.GetComponent<Healthbit>().Change(HealthBip.GetComponent<Healthbit>().State3);break;
        }
    }
    



    if(UIChange>=1f)
    {
        LastChange+=1;
        UIChange=0;
    }
}

Vector3 DeathPosition;
float Anim_death_Shake;
bool Anim_death_fling;
bool Anim_death_fling_done;
void UpdateDeath()
{
    if(Anim_death_time>1.8f && !Anim_death_fling_done){ 
        OnGameOver?.Invoke(true);
        Anim_death_fling_done = true;
    }
    if(Anim_death_time == 0f) DeathPosition = transform.position;

    Anim_death_time += Time.deltaTime;
    Stock_Sprite.GetComponent<Collider2D>().enabled = false;
        Brute_Sprite.GetComponent<Collider2D>().enabled = false;
    if(Anim_death_time<1f){
    if (Mathf.Floor(Anim_death_time/0.1f) != Anim_death_Shake){
        transform.position = DeathPosition + new Vector3(Random.Range(-1f,1f)*0.2f,Random.Range(-1f,1f)*0.1f,0);
        float r = Random.Range(-5f,5f)*12f;
    Stock_Sprite.transform.rotation =Quaternion.Euler(0,0,r);
    Brute_Sprite.transform.rotation =Quaternion.Euler(0,0,r);
    }
    Anim_death_Shake = Mathf.Floor(Anim_death_time/0.1f);
    rb.velocity=Vector2.zero;
    }else{
        
        if (!Anim_death_fling){
            Stock_Sprite.transform.rotation =Quaternion.Euler(0,0,0);
            Brute_Sprite.transform.rotation =Quaternion.Euler(0,0,0);
            
            float fling = Vector2.Angle(DeathPosition,transform.position+new Vector3(0.6f,1f,0f));
            
            rb.velocity = new Vector2(Mathf.Cos(fling)*2f,Mathf.Sin(fling)*2f)*5f;
            Anim_death_fling = true;
        } 
        Stock_Sprite.transform.rotation =Quaternion.Euler(0,0,Anim_death_time-1f*30f);
        Brute_Sprite.transform.rotation =Quaternion.Euler(0,0,Anim_death_time-1f*30f);
    }
    Anim_death_Shake = Mathf.Floor(Anim_death_time/0.1f);
}


KeyCode Stock_Dash = KeyCode.LeftShift;
KeyCode Brute_Punch = KeyCode.F;
KeyCode Duo_Swap = KeyCode.Q;
KeyCode Duo_Up = KeyCode.W;
KeyCode Duo_Down = KeyCode.S;
KeyCode Duo_Left = KeyCode.A;
KeyCode Duo_Right = KeyCode.D;


//feels bad man
void Update_Brute()
{
    PunchTimer-=Time.deltaTime;
        if (Input.GetKeyDown(Brute_Punch)) 
        {
       if(SlamEnabled){
        DashDirection = Vector2.down;
       }
       // DashPower = 190f;
        
        PunchTimer = 4f; 
        }
        if(!SlamActive && PunchTimer>=0f)
        {
            SlamActive = true;
            OutofDashState.VelocityOutside = Velocity;
            OutofDashState.AccelerationTime = accelerationTime;
            Debug.Log("Slam Started");
     
            //Stock_Sprite.GetComponent<ParticleSystem>().Play();
        }
        else if(SlamActive && PunchTimer<=0f)
        {
            Debug.Log("Slam Ended");
            SlamActive = false;
           
            //Stock_Sprite.GetComponent<ParticleSystem>().Stop();
        }else if(PunchTimer>0f && SlamActive){
            UpdateTrail();
            PunchBox.transform.localPosition = Vector3.down*Brute_Sprite.GetComponent<BoxCollider2D>().size.y/2f;
        }
        if(SlamActive && CollisionState == Collision_State.OnGround)
        {
        Debug.Log("Slam Abrupted");
        PunchTimer=0;
        rb.velocity = new Vector2(Velocity.x,JumpPower/5f);
        }
        PunchBox.SetActive(PunchTimer>0f);
        PunchTimer-=Time.deltaTime*(SlamActive?0f:1f);
        
        OutofDashState.VelocityInside = DashDirection  * 6.9f * 4f; //* Mathf.Pow(1f-(DashPower-182)/8f,1.2f);
        rb.gravityScale = !SlamActive?1f:0f;
        BruteHealth = Health;
}

//Special Child...
void Update_Stock()
{
        DashPower-=Time.deltaTime;
        if (Input.GetKeyDown(Stock_Dash) && DashEnabled) 
        {
        DashUsedInAir = CollisionState == Collision_State.Airborne||CollisionState == Collision_State.BelowCeiling;
        DashDirection = new Vector2(Input.GetKey(Duo_Left)?-1f:Input.GetKey(Duo_Right)?1f:0,Input.GetKey(Duo_Down)?-1f:Input.GetKey(Duo_Up)?1f:0);
        Stock_Sprite.GetComponent<SpriteRenderer>().flipX = Input.GetKey("a");
                Stock_Sprite.GetComponent<SpriteRenderer>().flipY = Input.GetKey("s");
        if (DashDirection.magnitude==0) DashDirection = Vector2.left*(Stock_Sprite.GetComponent<SpriteRenderer>().flipX?1:-1);
       // DashPower = 190f;
        DashPower = 0.5f;
        DashCooldown = DashPower*1.2f; 
        }
        if(!DashActive && DashPower>0f)
        {
            DashActive = true;
            OutofDashState.VelocityOutside = Velocity;
            OutofDashState.AccelerationTime = accelerationTime;
            Debug.Log("Dash Started");
            Debug.Log(CollisionState);
            //Stock_Sprite.GetComponent<ParticleSystem>().Play();
        }
        else if(DashActive && DashPower<=0f)
        {
            Debug.Log("Dash Ended");
            DashActive = false;
           
            //Stock_Sprite.GetComponent<ParticleSystem>().Stop();
        }else if(DashPower>0f){
            UpdateTrail();
         
        }
        if(DashUsedInAir && (CollisionState != Collision_State.Airborne || Previous == Collision_State.AgainstWall))
        {
        Debug.Log("Dash Abrupted");
        DashPower=0f;
        DashCooldown=0.1f;
        DashUsedInAir=false;

        }
        
        DashCooldown-=Time.deltaTime*(DashUsedInAir?0f:1f);
        
        OutofDashState.VelocityInside = DashDirection * WalkSpeed * 1.5f; //* Mathf.Pow(1f-(DashPower-182)/8f,1.2f);
        rb.gravityScale = DashPower<=0f?1f:0f;
        StockHealth = Health;
}


// Update is called once per frame
    new void Update()
    {
        UpdateUI();
        if(Duo_Dead){
            UpdateDeath();
            return;
        }


         if(Input.GetKey(KeyCode.P) && IFrame_Ticker<=0){
            takeDamage(100);
        }
        base.Update();
        UpdateDamage();
        
        
        //Change character sprite
        if (Input.GetKeyDown(Duo_Swap) &&CanSwap)
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
                JumpPower = 5.25f;
                Health = BruteHealth;
                IsStock = false; 
            }
          
        }
       
        UIChange=Mathf.Clamp(UIChange+Time.deltaTime*(UIChange>0f?1:0), 0f,1f);
        if(IsStock) Update_Stock(); else Update_Brute();
        //Fight or Flight
       /* PunchBox.SetActive(PunchBoxTimer>0&&PunchBoxTimer<0.4);
        
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
        {*/
            //PunchBox.transform.localPosition = new Vector3(1.026f*(Brute_Sprite.GetComponent<SpriteRenderer>().flipX?-1:1)*-1f,0.202f,0);
            GetAnimator().SetBool("Attacking",false);
          
            Stock_Sprite.GetComponent<SpriteRenderer>().flipY =false;
            Brute_Sprite.GetComponent<SpriteRenderer>().flipY =false;

            if (Input.GetKey(Duo_Left) ^ Input.GetKey(Duo_Right)) //This is and XOR if statement
            {
                accelerationTime+= Time.deltaTime*acceleration/3f;
                Velocity.x += (acceleration * Time.deltaTime + accelerationTime / 5) * (Input.GetKey(Duo_Left) ? -1f : 1f)/(CollisionState==Collision_State.OnGround?1f:4f);
                Stock_Sprite.GetComponent<SpriteRenderer>().flipX = Input.GetKey(Duo_Left);
                Brute_Sprite.GetComponent<SpriteRenderer>().flipX = Input.GetKey(Duo_Left);
            }
            else
            {
                accelerationTime-= Time.deltaTime*deceleration;
                Velocity.x -= deceleration/(CollisionState==Collision_State.OnGround?1f:4f) * Mathf.Sign(Velocity.x) * Time.deltaTime;
            }
       
            GetAnimator().SetInteger("Air State",Is_OnGround?rb.velocity.y<0?2:0:DashPower>0?3:rb.velocity.y<0?2:1);
            GetAnimator().SetBool("Moving", accelerationTime>0);

            accelerationTime = Mathf.Clamp(accelerationTime, 0f,1f);

            Velocity.x = Mathf.Clamp(Velocity.x, -1f, 1f);
            
            Vector2 V = LocalVelocity;
           
          
            rb.velocity =  V;
        //}
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (Is_OnGround || Can_ExtraJump) // Checks if player is grounded then if doubleJump is true
            {
                tempVelocity = Vector2.zero;
                if(Can_ExtraJump && Is_EligibleForWallAction){ My_PreviousWall = Is_OnWhatWall;
                tempVelocity.x = (int)Is_OnWhatWall*JumpPower/1.25f*((int)Is_OnWhatWall==(Stock_Sprite.GetComponent<SpriteRenderer>().flipX?1f:-1f)?1f:1f);
                Can_DoubleJump =true;
                }else
                if(Can_ExtraJump && Can_DoubleJump) Can_DoubleJump =false;
                rb.velocity = new Vector2(Velocity.x,JumpPower);
            }
        }
        if (Input.GetKeyUp("space") && rb.velocity.y > 0f) // If space is let go mid-jump, upwards velocity halved for smaller jump
        {

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
         if (DashActive || SlamActive) rb.velocity = OutofDashState.VelocityInside;

         (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<SpriteRenderer>().color = new Color(
        (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<SpriteRenderer>().color.r,
         (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<SpriteRenderer>().color.g,
         (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<SpriteRenderer>().color.b,
         Mathf.Floor(IFrame_Ticker/2f)%2==1?0.8f:1f);
    }
    
    public void OnStateChange_Player(Collision_State From, Collision_State To){
        //Left the ground in some manor, either by jumping or walking off a
       //Debug.Log(From.ToString()+">"+To.ToString());
        if(From == Collision_State.OnGround && To == Collision_State.Airborne){
            Can_DoubleJump =true;
            if(IsStock){
            if(DashCooldown>0)DashCooldown = DashUsedInAir? 0.1f: DashCooldown - Time.deltaTime;
            DashUsedInAir = false;
            }
           

        }
        if(From == Collision_State.Airborne && !rb.IsDestroyed()){
            
            rb.velocity = new(0,rb.velocity.y);
            Brute_Sprite.GetComponent<SpriteRenderer>().color =Color.white;
            PunchBox.SetActive(false);
            
           
            }
        }
        
    }
    

