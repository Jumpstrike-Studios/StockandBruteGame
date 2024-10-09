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
        if (Input.GetKey("d"))
        {
            move = 1;
        }
        else if (Input.GetKey("a"))
        {
            move = -1;
        }
        else {
            move = 0;
        }

        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        //Jump and double jump mechanic
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded || doubleJump) // Checks if player is grounded then if doubleJump is true
            {
                rb.velocity = Vector2.up * jump;
                doubleJump = !doubleJump;
                isGrounded = false;
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
            doubleJump = false;
        }
    }
}