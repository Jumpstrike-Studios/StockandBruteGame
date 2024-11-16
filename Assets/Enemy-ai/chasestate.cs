using UnityEngine;

public class ChaseState : State
{
    private EnemyAI enemy;

    public ChaseState(StateMachine stateMachine) : base(stateMachine)
    {
        enemy = (EnemyAI)stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entered Chase State");
    }

    public override void UpdateLogic()
    {
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distanceToPlayer > enemy.detectionRange)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
        }
        else if (distanceToPlayer < enemy.attackRange)
        {
            stateMachine.ChangeState(new AttackState(stateMachine));
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
        enemy.rb.velocity = direction * enemy.moveSpeed;
    }

    public override void Exit()
    {
        enemy.rb.velocity = Vector2.zero;
    }
}
