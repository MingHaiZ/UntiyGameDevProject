using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    [Space]
    [SerializeField] private int amountOfAttacks;

    [SerializeField] private float cloneAttackCoolDown;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void useSkill()
    {
        base.useSkill();
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        Blackhole_Skill_Controller newBlackHoleScript = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        newBlackHoleScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCoolDown);
    }
}