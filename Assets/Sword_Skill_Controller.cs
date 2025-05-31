using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        player = PlayerManager.instance.player;
        Debug.Log(anim.name + rb.name + cd.name + player.name);
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }
}