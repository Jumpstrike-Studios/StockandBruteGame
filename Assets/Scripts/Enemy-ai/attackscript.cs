using UnityEngine;

public class AttackState : State
{
    private EnemyAI enemy;
    private float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
        enemy = (EnemyAI)stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entered Attack State");
    }

    public override void UpdateLogic()
    {
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distanceToPlayer > enemy.attackRange)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Enemy Attacks!");
        nextAttackTime = Time.time + attackCooldown;
        // Placeholder for actual attack logic (e.g., dealing damage)
    }
}
