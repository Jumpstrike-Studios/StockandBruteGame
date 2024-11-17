using UnityEngine;

public class BreakableWall : Actor
{
  
    [Header("Death Effects")]
    public GameObject deathEffect; // Optional: Particle effect on break
    public Animator animator; // Reference to the wall's Animator
    public float destroyDelay = 1f; // Delay before the wall is destroyed

    private void Start()
    {
       Health = new ActorVitals(50);
    }

    // apply damage to the wall
    public void TakeDamage(int damage)
    {
        Health.Health -= damage;
        Debug.Log($"Wall took {damage} damage. Current health: {Health.Health}");

        // break the wall
        if (Health.Health <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        Debug.Log("Wall is broken!");

        // Trigger the "Die" animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable the collider
        GetComponent<Collider2D>().enabled = false;

        // Destroy the wall after a short delay to allow animation/particle effects
        Destroy(gameObject, destroyDelay);
    }
}
