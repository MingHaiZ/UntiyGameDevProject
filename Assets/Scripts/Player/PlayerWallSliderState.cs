using UnityEngine;

public class PlayerWallSliderState : PlayerState
{
    public PlayerWallSliderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        } else
        {
            player.SetVelocity(0, rb.velocity.y * player._wallSliderSpeed);
        }

        if (player.IsGroundedDetected() || !player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}