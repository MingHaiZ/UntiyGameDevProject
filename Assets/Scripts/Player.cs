using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public bool isBusy { get; private set; }

    [Header("Move Info")]
    public float _moveSpeed;

    public float _jumpForce;
    public float _wallSliderSpeed;

    [Header("Dash Info")]
    public float dashSpeed;

    public float dashDuration;

    public float dashDir { get; private set; }
    [SerializeField] private float dashCooldown;
    private float dashUseageTimer;

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;

    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallDistance;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;


    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion

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

    #endregion

    public void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, PlayerStateConstants.PlayerIdle);
        moveState = new PlayerMoveState(this, stateMachine, PlayerStateConstants.PlayerMove);
        jumpState = new PlayerJumpState(this, stateMachine, PlayerStateConstants.PlayerJump);
        airState = new PlayerAirState(this, stateMachine, PlayerStateConstants.PlayerJump);
        dashState = new PlayerDashState(this, stateMachine, PlayerStateConstants.PlayerDash);
        wallSliderState = new PlayerWallSliderState(this, stateMachine, PlayerStateConstants.PlayerWallSlider);
        WallJumpState = new PlayerWallJumpState(this, stateMachine, PlayerStateConstants.PlayerJump);
        PrimaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, PlayerStateConstants.PlayerAttack);
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    #region Velocity

    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    #endregion
    
    # region Collsion

    public bool IsGroundedDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, whatIsGround);

    public bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallDistance, whatIsGround);

    #endregion

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinshTrigger();

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + (wallDistance * facingDir), wallCheck.position.y));
    }

    # region Flip

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180 * facingDir, 0);
    }

    public void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        } else if (x < 0 && facingRight)
        {
            Flip();
        }
    }

    #endregion

    private void CheckForDashInput()
    {
        dashUseageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUseageTimer <= 0 && !IsWallDetected())
        {
            dashUseageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }
}