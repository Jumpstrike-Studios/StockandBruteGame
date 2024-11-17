using UnityEngine;

public class EnemyAI : StateMachine
{
    [HideInInspector] public Rigidbody2D rb;
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start with the PatrolState
        ChangeState(new PatrolState(this));
    }
}
