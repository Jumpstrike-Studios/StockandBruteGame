using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Actor_Player : Actor
{
    private float JumpPower = 5f;
    private bool doubleJump;
    public Sprite Stock_Sprite;
    public Sprite Brute_Sprite;
    public float acceleration;
    public float maxVelocity;
    public float deceleration;

// Update is called once per frame
void Update()
    {
        //Change character sprite
         if (Input.GetKeyDown("q"))
        {
            if (GetComponent<SpriteRenderer>().sprite == Brute_Sprite)
            {
                GetComponent<SpriteRenderer>().sprite = Stock_Sprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Brute_Sprite;
            }
        }


        if (Input.GetKey("d"))
        {
            Velocity.x += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey("a"))
        {
            Velocity.x -= acceleration * Time.deltaTime;
        }
        else if (Velocity.x != 0f) 
        {
             Velocity.x -= deceleration * (Mathf.Sign(Velocity.x)) * Time.deltaTime;
        }

        // Clamps to max velocity
        Velocity.x = Mathf.Clamp(Velocity.x, -maxVelocity, maxVelocity);

        rb.velocity = new Vector2(Velocity.x * WalkSpeed, rb.velocity.y);
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded || doubleJump) // Checks if player is grounded then if doubleJump is true
            {
                rb.velocity = Vector2.up * JumpPower;
                doubleJump = !doubleJump;
                isGrounded = false;
            }
        }
        if (Input.GetKeyUp("space") && rb.velocity.y > 0f) // If space is let go mid-jump, upwards velocity halved for smaller jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Ground")){
            doubleJump = false;
        }
    }
}