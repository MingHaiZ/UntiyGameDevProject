public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy)
        : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(24, enemy.transform);
    }
}