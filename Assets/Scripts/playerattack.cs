using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int attackDamage = 20; // Damage dealt to enemies
    public float attackRange = 1f; // Range of the attack
    public LayerMask enemyLayer; // Layer to detect enemies
    public Transform attackPoint; // The point from which the attack is performed
    public float attackCooldown = 0.5f; // Time between attacks

    private float nextAttackTime = 0f;

    private void Update()
    {
        // Attack when F is pressed and the cooldown is over
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void PerformAttack()
    {
        // Detect enemies within the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Apply damage to each enemy detected
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log($"Player dealt {attackDamage} damage to {enemyCollider.gameObject.name}");
            }
        }
    }

    // Visualize the attack range in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
