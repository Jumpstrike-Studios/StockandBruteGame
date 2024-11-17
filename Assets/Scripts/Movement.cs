using System;
using System.Collections;
using System.Collections.Generic;
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

    public ActorVitals StockHealth;
    public ActorVitals BruteHealth;
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
                JumpPower = 3.5f;
                Health = BruteHealth;
                IsStock = false;

                Vector3 oldPosition = BruteHealthBar.transform.position;

                BruteHealthBar.transform.position = StockHealthBar.transform.position;
                BruteHealthBar.transform.localScale = BruteHealthBar.transform.localScale * 2;
                StockHealthBar.transform.localScale = StockHealthBar.transform.localScale / 2;
                StockHealthBar.transform.position = oldPosition;

               
            }
        }


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
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded) // Checks if player is grounded then if doubleJump is true
            {
                rb.velocity = new Vector2(rb.velocity.x,JumpPower);
                doubleJump = !doubleJump;
                isGrounded = false;
            }
            else if (IsStock && doubleJump)
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
           
        }
    }
}