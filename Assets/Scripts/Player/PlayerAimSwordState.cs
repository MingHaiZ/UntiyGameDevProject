using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Skill.sword.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.velocity.y);
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }

        Vector2 moustPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > moustPosition.x && player.facingDir == 1)
        {
            player.Flip();
        } else if (player.transform.position.x < moustPosition.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.3f);
    }
}