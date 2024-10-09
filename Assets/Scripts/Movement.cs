using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public float speed;
    private float move;
    private float jump = 5f;
    bool isGrounded;
    private bool doubleJump;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

// Update is called once per frame
void Update()
    {
        move = Input.GetAxis("Horizontal");
        if (isGrounded && !Input.GetKeyDown("space")) // Checks to see if player is grounded and if the jump button is not pressed
        {
            doubleJump = false;
        }

        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (Input.GetKeyDown("space"))
        {
            if (isGrounded || doubleJump) // Checks if player is grounded then if doubleJump is true
            {
               if (doubleJump == false)
                {
                    rb.velocity = Vector2.up * jump;
                }
               if (doubleJump == true)
                {
                    rb.velocity = Vector2.up * jump;
                }

               doubleJump = !doubleJump;
            }                                
        }
        if (Input.GetKeyUp("space") && rb.velocity.y > 0f) // If space is let go mid-jump, upwards velocity halved for smaller jump
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

    }
void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

void OnCollisionExit2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("Ground")) // When collision with the ground is lost, doubleJump is true, enabling another jump
        {
            isGrounded = false;
            doubleJump = true;
        }
    }
}
