using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Skill.clone.CreateClone(player.transform);

        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundedDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSliderState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (stateTimer < 0 && player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.idleState);
        } else if (stateTimer < 0 && !player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }
}