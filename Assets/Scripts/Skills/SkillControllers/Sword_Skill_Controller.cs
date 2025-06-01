using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    [Header("Bouncing Info")]
    [SerializeField] private float bounceSpeed;

    private bool isBouncing;
    private int amountOfBounce;

    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    [SerializeField] private float pierceAmount;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position,
                    bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount<=0)
        {
            anim.SetBool("Rotation", true);
        }
        
    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounces)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }
        
        other.GetComponent<Enemy>()?.Damage();
        
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in collider)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }

        StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {
        if (pierceAmount > 0 && other.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}