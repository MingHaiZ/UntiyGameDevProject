using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public float counterAttackDuration = 0.2f;

    public bool isBusy { get; private set; }

    [Header("Move Info")]
    public float _moveSpeed;

    public float _jumpForce;
    public float _wallSliderSpeed;
    public float _sowrdReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

    [Header("Dash Info")]
    public float dashSpeed;

    public float dashDuration;

    public float dashDir { get; private set; }
    [SerializeField] private float dashCooldown;
    private float dashUseageTimer;

    #region States

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSliderState wallSliderState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    public SkillManager Skill = SkillManager.instance;
    public GameObject sword { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, PlayerStateConstants.PlayerIdle);
        moveState = new PlayerMoveState(this, stateMachine, PlayerStateConstants.PlayerMove);
        jumpState = new PlayerJumpState(this, stateMachine, PlayerStateConstants.PlayerJump);
        airState = new PlayerAirState(this, stateMachine, PlayerStateConstants.PlayerJump);
        dashState = new PlayerDashState(this, stateMachine, PlayerStateConstants.PlayerDash);
        wallSliderState = new PlayerWallSliderState(this, stateMachine, PlayerStateConstants.PlayerWallSlider);
        WallJumpState = new PlayerWallJumpState(this, stateMachine, PlayerStateConstants.PlayerJump);

        PrimaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, PlayerStateConstants.PlayerAttack);
        CounterAttackState = new PlayerCounterAttackState(this, stateMachine, PlayerStateConstants.PlayerCounterAttack);

        AimSwordState = new PlayerAimSwordState(this, stateMachine, PlayerStateConstants.PlayerAimSword);
        CatchSwordState = new PlayerCatchSwordState(this, stateMachine, PlayerStateConstants.PlayerCatchSword);
        BlackholeState = new PlayerBlackholeState(this, stateMachine, PlayerStateConstants.PlayerJump);
        deadState = new PlayerDeadState(this, stateMachine, PlayerStateConstants.PlayerDead);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = _moveSpeed;
        defaultJumpForce = _jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && stateMachine.currentState != BlackholeState)
        {
            Skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        _moveSpeed = _moveSpeed * (1 - _slowPercentage);
        _jumpForce = _jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        _moveSpeed = defaultMoveSpeed;
        _jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(CatchSwordState);
        Destroy(sword);
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinshTrigger();


    private void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Skill.dash.CanUseSkill() && !IsWallDetected())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}