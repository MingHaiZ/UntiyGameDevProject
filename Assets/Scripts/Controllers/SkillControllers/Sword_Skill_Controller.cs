using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    [Header("Bouncing Info")]
    private float bounceSpeed;

    private bool isBouncing;
    private int amountOfBounce;

    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    private float pierceAmount;

    [Header("Spin Info")]
    private float maxTravelDistance;

    private float spinDuration;
    private float spinTimer;
    private float spinGravity = 1;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCoolDown;

    private float spinDirection;

    private float freezeDuration;
    private float returnSpeed = 12f;

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
        SpinLogic();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }


    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;
                    Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in collider)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
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
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

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

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration,
        float _returnSpeed)
    {
        player = _player;
        freezeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
        {
            anim.SetBool("Rotation", true);
        }

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        Invoke(nameof(DestroyMe), 7);
    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetUpSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _spinGravity,
        float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        spinGravity = _spinGravity;
        hitTimer = _hitCoolDown;
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

        if (other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }


        SetUpTargetForBounce(other);

        StuckInto(other);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());

        if (player.Skill.sword.timeStopUnlocked)
        {
            enemy.FreezeTimeFor(freezeDuration);
        }

        if (player.Skill.sword.vulnerableUnlocked)
        {
            enemyStats.MakeVulnerableFor(freezeDuration);
        }

        ItemData_Equipment equipment = Inventory.instance.GetEquipmentType(EquipmentType.Amulet);

        if (equipment != null)
        {
            equipment.Effect(enemy.transform);
        }
    }

    private void SetUpTargetForBounce(Collider2D other)
    {
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

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}