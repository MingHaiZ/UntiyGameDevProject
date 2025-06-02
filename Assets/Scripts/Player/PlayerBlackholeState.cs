using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.Skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////
        //  We exit state in blackhole skill controller when all of the attacks are over///
        ///////////////////////////////////////////////////////////////////////////////////
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.MakeTransprent(false);
    }

    public override void AnimationFinshTrigger()
    {
        base.AnimationFinshTrigger();
    }
}