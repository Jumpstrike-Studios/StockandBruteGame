using UnityEngine;

public class PatrolState : State
{
    private EnemyAI enemy;
    private Vector2 patrolPointA;
    private Vector2 patrolPointB;
    private Vector2 targetPoint;
    private bool movingToA = true;

    public PatrolState(StateMachine stateMachine) : base(stateMachine)
    {
        enemy = (EnemyAI)stateMachine;
        patrolPointA = new Vector2(enemy.transform.position.x - 3, enemy.transform.position.y);
        patrolPointB = new Vector2(enemy.transform.position.x + 3, enemy.transform.position.y);
        targetPoint = patrolPointA;
    }

    public override void Enter()
    {
        Debug.Log("Entered Patrol State");
    }

    public override void UpdateLogic()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRange)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }

        Patrol();
    }

    private void Patrol()
    {
        if (Vector2.Distance(enemy.transform.position, targetPoint) < 0.1f)
        {
            movingToA = !movingToA;
            targetPoint = movingToA ? patrolPointA : patrolPointB;
        }

        Vector2 direction = (targetPoint - (Vector2)enemy.transform.position).normalized;
        enemy.rb.velocity = direction * enemy.moveSpeed;
    }

    public override void Exit()
    {
        enemy.rb.velocity = Vector2.zero;
    }
}
