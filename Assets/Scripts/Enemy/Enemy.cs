using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    public float stunDuration;

    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;

    public float idleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;

    public float attackCoolDown;
    public float battleTime;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }


    public virtual void FreezeTime(bool _timerFrozen)
    {
        if (_timerFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        } else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }


    public virtual void AnimationFinshTrigger() => stateMachine.currentState.AnimationFinshTrigger();

    public virtual RaycastHit2D IsPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}