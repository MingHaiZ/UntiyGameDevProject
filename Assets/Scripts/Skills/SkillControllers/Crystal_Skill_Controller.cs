using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMoveToEnemy;
    private float moveSpeed;
    private Animator anim;
    private bool canGrow;
    private float growSpeed = 5;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMove;
        moveSpeed = _moveSpeed;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        } else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
}