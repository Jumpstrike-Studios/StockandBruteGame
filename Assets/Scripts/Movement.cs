using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Actor_Player : Actor
{
    public GameObject trail_root;
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

    private bool doubleJump;
    private bool IsStock=false;
    private bool HasDashed;

    private float UIChange;

    
    private Vector2 DashDirection;

new public void Start()
{
base.Start();
 StockHealth = new ActorVitals(200);
 BruteHealth = new ActorVitals(500);
 Health = BruteHealth;
 StockHealth.RemoveOnDeath=false;
 BruteHealth.RemoveOnDeath=false;
}

public void UpdateTrail(){
    TrailTimer-=Time.deltaTime;
    if(TrailTimer < 0){
       GameObject trail = Instantiate(trail_root,IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().transform.position:Brute_Sprite.GetComponent<SpriteRenderer>().transform.position,transform.rotation);
        trail.GetComponent<SpriteRenderer>().sprite = IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().sprite:Brute_Sprite.GetComponent<SpriteRenderer>().sprite;
        trail.GetComponent<SpriteRenderer>().flipX = IsStock?Stock_Sprite.GetComponent<SpriteRenderer>().flipX:Brute_Sprite.GetComponent<SpriteRenderer>().flipX;
        TrailTimer=0.09f;
    }
}

public Animator GetAnimator(){return (IsStock?Stock_Sprite:Brute_Sprite).GetComponent<Animator>();}

void UpdateUI()
{
    Vector3 AnchorPoint = new Vector3(0, 0, 0);
    float TrigTimer = UIChange * Mathf.PI;
    Vector3 FinalPosition(float Offset)
    {
        return new Vector3(Mathf.Sin(TrigTimer + Offset * Mathf.PI), Mathf.Cos(TrigTimer + Offset * Mathf.PI), 0) + AnchorPoint;
    }
    StockHealthBar.transform.localPosition = FinalPosition(0);
    BruteHealthBar.transform.localPosition = FinalPosition(1);
}
// Update is called once per frame
void Update()
    {
        float dt = Time.deltaTime;
        curDashCooldown -= dt;

        if (HasDashed && curDashCooldown < 0 && isGrounded)
        {
            HasDashed = false;
            DashTimer = 0;
        }

        UpdateUI();
        UIChange=Mathf.Clamp(UIChange-Time.deltaTime*(IsStock?-1f:1f), 0f,1f);
        //Change character sprite
        if (Input.GetKeyDown("q") && DashTimer<=0 && PunchBoxTimer<=0)
        {
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
        if (Input.GetKey("f"))
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
            else if(!IsStock && PunchBoxTimer<=0)
            {
                if(isGrounded)
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

            GetAnimator().SetInteger("Air State",isGrounded?rb.velocity.y<0?2:0:DashTimer>0?3:rb.velocity.y<0?2:1);
            GetAnimator().SetBool("Moving", accelerationTime>0);
            accelerationTime = Mathf.Clamp(accelerationTime, 0f,1f);
            Velocity.x = Mathf.Clamp(Velocity.x, -1f, 1f);
            rb.velocity = new Vector2(Velocity.x * WalkSpeed*accelerationTime, rb.velocity.y);
        }
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded || (IsStock && doubleJump)) // Checks if player is grounded then if doubleJump is true
            {
                rb.velocity = new Vector2(rb.velocity.x,JumpPower);
                doubleJump = !doubleJump;
                isGrounded = false;
            }
        }
        if (Input.GetKeyUp("space") && rb.velocity.y > 0f) // If space is let go mid-jump, upwards velocity halved for smaller jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    new public void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Ground")){
            doubleJump = false;
            DashTimer = 0;
            PunchBoxTimer=0;
            HasDashed=false;
        }
        if (collision.gameObject.CompareTag("Wall") && IsStock){
            doubleJump = false;
            DashTimer = 0;
            PunchBoxTimer = 0;
            HasDashed = false;
            isGrounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}