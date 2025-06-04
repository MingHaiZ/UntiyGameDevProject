using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName,
        Enemy_Skeleton _enemy) : base(stateMachine,
        enemyBase, animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinshTrigger()
    {
        base.AnimationFinshTrigger();
    }
}