using UnityEngine;
/// <summary>
/// The basis of everything that moves and/or is interactable
/// </summary>
public class Actor : MonoBehaviour
{
    int MaxHealth=100;
    public bool Invincible{
        get { return (MaxHealth<0);}
    }
    [Header("General")]
    /// <summary>
    /// The health of the actor. This value sets both the maximum hp and the invincibility flag
    /// </summary>
    public int Health=100;
    
    public float WalkSpeed = 5f;
    [Range(0f,1f)]
    public float GravityScale=1f;
    protected Vector2 Velocity;
    protected bool isGrounded;
    protected Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxHealth = Health;
        rb = GetComponent<Rigidbody2D>();
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
