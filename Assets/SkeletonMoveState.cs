using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        if (enemy.IsWallDetected() || !enemy.IsGroundedDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}