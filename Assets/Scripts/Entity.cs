using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;

    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;

    [SerializeField] protected float groundDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallDistance;
    public Transform attackCheck;
    public float attackCheckRadius;
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;


    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #endregion

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        
    }

    protected virtual void Update()
    {
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("flashFX");
        StartCoroutine(HitKnockback());
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    # region Collsion

    public bool IsGroundedDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, whatIsGround);

    public bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallDistance, whatIsGround);

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + (wallDistance * facingDir), wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    # region Flip

    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180 * facingDir, 0);
    }

    public virtual void FlipController(float x)
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

    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    #endregion

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            sr.color = Color.clear;
        } else
        {
            sr.color = Color.white;
        }
    }
}