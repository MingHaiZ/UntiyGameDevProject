using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
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
        if (player.stateMachine.currentState == player.deadState)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && player.Skill.blackhole.blackHoleUnlocked &&
            !player.Skill.blackhole.IsCoolDown())
        {
            stateMachine.ChangeState(player.BlackholeState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.Skill.sword.swordUnlocked)
        {
            stateMachine.ChangeState(player.AimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.Skill.parry.parryUnlocked)
        {
            stateMachine.ChangeState(player.CounterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}