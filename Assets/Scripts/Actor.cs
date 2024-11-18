
using UnityEngine;
/// <summary>
/// The basis of everything that moves and/or is interactable
/// </summary>
public class Actor : MonoBehaviour
{
    public struct ActorVitals
    {
        public int MaxHealth;
        public int Health;
       public bool Invincible{
        get { return MaxHealth<0;}
    
    }
    /// <summary>
    /// When the actor's health reaches 0, the actor will be removed from the scene
    /// </summary>
    public bool RemoveOnDeath;
    public ActorVitals(int BaseHealth)
    {
        MaxHealth = BaseHealth;
        Health = BaseHealth;
        RemoveOnDeath=true;
    }
    }
    [Header("General")]
    /// <summary>
    /// The health of the actor. This value sets both the maximum hp and the invincibility flag
    /// </summary>
    public ActorVitals Health;
    public float WalkSpeed = 5f;
    [Range(0f,1f)]
    public float GravityScale=1f;
    protected Vector2 Velocity;
    protected bool isGrounded;
    protected Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Health=new ActorVitals(100);
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
    public void Die()
    {
        if(Health.RemoveOnDeath)
        {
            Destroy(this);
        }
    }
}
