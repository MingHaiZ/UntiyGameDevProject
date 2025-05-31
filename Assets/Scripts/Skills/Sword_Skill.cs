using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = sword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetUpSword(launchDir, swordGravity);
    }
}