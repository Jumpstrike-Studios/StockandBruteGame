using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Actor_Player : Actor
{
    private float JumpPower = 5f;
    private bool doubleJump;


// Update is called once per frame
void Update()
    {
        if (Input.GetKey("d"))
        {
            Velocity.x = 1;
        }
        else if (Input.GetKey("a"))
        {
            Velocity.x = -1;
        }
        else {
            Velocity.x = 0;
        }

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