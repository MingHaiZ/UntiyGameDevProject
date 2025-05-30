using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;

    public SkeletonGroundedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName,
        Enemy_Skeleton enemy) : base(
        stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}