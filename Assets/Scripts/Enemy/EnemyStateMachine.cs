public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }

    public virtual void InitializeState(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public virtual void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}