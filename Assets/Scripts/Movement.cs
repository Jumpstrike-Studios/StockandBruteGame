using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Actor_Player : Actor
{
    public float JumpPower = 3.5f;
    private bool doubleJump;
    public GameObject Stock_Sprite;
    public GameObject Brute_Sprite;
    public float acceleration;
    public float deceleration;
    private bool IsStock=false;
    float accelerationTime;
    public GameObject StockHealthBar;
    public GameObject BruteHealthBar;

     public GameObject PunchBox;
     private float PunchBoxTimer;

    public ActorVitals StockHealth;
    public ActorVitals BruteHealth;

    private float DashTimer;
    private Vector2 DashDirection;

    private bool HasDashed;
new public void Start()
{
base.Start();
 StockHealth = new ActorVitals(200);
 BruteHealth = new ActorVitals(500);
 Health = BruteHealth;
 StockHealth.RemoveOnDeath=false;
 BruteHealth.RemoveOnDeath=false;
}

// Update is called once per frame
void Update()
    {
        //Change character sprite
         if (Input.GetKeyDown("q"))
        {
            if (!IsStock)
            {

                // Changes sprite to stock
                Stock_Sprite.SetActive(true); 
                Brute_Sprite.SetActive(false);
                
                JumpPower = 7.0f;
                Health = StockHealth;
                IsStock = true;

                Vector3 oldPosition = StockHealthBar.transform.position;

                StockHealthBar.transform.position = BruteHealthBar.transform.position;
                StockHealthBar.transform.localScale = StockHealthBar.transform.localScale * 2;
                BruteHealthBar.transform.localScale = BruteHealthBar.transform.localScale / 2;
                BruteHealthBar.transform.position = oldPosition;



            }
            else
            {

                // Changes sprite to brute
                Stock_Sprite.SetActive(false);
                Brute_Sprite.SetActive(true);
                JumpPower = 4.5f;
                Health = BruteHealth;
                IsStock = false;

                Vector3 oldPosition = BruteHealthBar.transform.position;

                BruteHealthBar.transform.position = StockHealthBar.transform.position;
                BruteHealthBar.transform.localScale = BruteHealthBar.transform.localScale * 2;
                StockHealthBar.transform.localScale = StockHealthBar.transform.localScale / 2;
                StockHealthBar.transform.position = oldPosition;

               
            }
        }
        //Fight or Flight
        if (Input.GetKey("f"))
        {
            if(IsStock && !isGrounded && !HasDashed && DashTimer<=0)
            {
            DashDirection = new Vector2(Input.GetKey("a")?-1f:Input.GetKey("d")?1f:0,Input.GetKey("s")?-1f:Input.GetKey("w")?1f:0);
            Stock_Sprite.GetComponent<SpriteRenderer>().flipX = Input.GetKey("a");
            Stock_Sprite.GetComponent<SpriteRenderer>().flipY = Input.GetKey("s");
            DashTimer = 0.5f;
            HasDashed = true;
            }else if(!IsStock && PunchBoxTimer<=0)
            {
                if(isGrounded){
                PunchBoxTimer=0.4f;
                PunchBox.transform.localPosition = new Vector3(1.026f*(Brute_Sprite.GetComponent<SpriteRenderer>().flipX?-1:1),0.202f,0);
                }else{
                    DashTimer = 1f;
                    PunchBoxTimer=1f;
                    DashDirection = Vector2.down;
                    Brute_Sprite.GetComponent<SpriteRenderer>().flipY = true;
                    PunchBox.transform.localPosition = new Vector3(0,-1f,0);
                }

            }

        }
        if(PunchBoxTimer>0)PunchBoxTimer-=Time.deltaTime;
        PunchBox.SetActive(PunchBoxTimer>0);
        Brute_Sprite.GetComponent<SpriteRenderer>().color = PunchBoxTimer>0?Color.red:Color.white;
        //Silly
        //transform.localScale = Vector2.one+new Vector2(Mathf.Abs(DashDirection.x),Mathf.Abs(DashDirection.y))*DashTimer*2;
        if(DashTimer>0)
        {
            DashTimer-=Time.deltaTime;
            rb.velocity = DashDirection*acceleration*math.clamp(DashTimer*4,0f,1f)*1.5f;
            rb.gravityScale=0;
            
        }else{
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
        accelerationTime = Mathf.Clamp(accelerationTime, 0f,1f);
        Velocity.x = Mathf.Clamp(Velocity.x, -1f, 1f);

        rb.velocity = new Vector2(Velocity.x * WalkSpeed*accelerationTime, rb.velocity.y);
        }
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded||(IsStock && doubleJump)) // Checks if player is grounded then if doubleJump is true
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
            HasDashed=false;
        }
    }
}