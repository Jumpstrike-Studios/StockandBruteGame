using UnityEngine;
/// <summary>
/// The basis of evil
/// </summary>
public class Enemy_Base : Actor
{

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        
        }
    }
}