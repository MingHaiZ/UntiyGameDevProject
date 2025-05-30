using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
        
        if (xInput == player.facingDir && player.IsWallDetected())
        {
            return;
        }

        if (!player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        } else
        {
            player.FlipController(xInput);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}