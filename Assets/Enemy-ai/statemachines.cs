using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.HandleInput();
        currentState?.UpdateLogic();
    }

    private void FixedUpdate()
    {
        currentState?.UpdatePhysics();
    }
}
