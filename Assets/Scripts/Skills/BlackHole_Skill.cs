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
    [SerializeField] private float blackholeDuration;

    private Blackhole_Skill_Controller currentBlackHole;


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
        currentBlackHole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackHole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCoolDown,
            blackholeDuration);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackHole)
        {
            return false;
        }

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}