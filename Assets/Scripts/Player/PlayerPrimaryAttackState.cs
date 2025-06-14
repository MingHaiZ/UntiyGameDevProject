using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 1;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(
        _player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // attack sound effect
        // AudioManager.instance.PlaySFX(2);
        
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger(PlayerStateConstants.PlayerComboCounter, comboCounter);

        #region Choose attack direction

        float attackDir = player.facingDir;
        if (xInput != 0)
        {
            attackDir = xInput;
        }

        #endregion

        player.SetVelocity(
            player.attackMovement[comboCounter].x * attackDir,
            player.attackMovement[comboCounter].y
        );
        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public void clearXInputCache()
    {
        xInput = 0;
    }
}