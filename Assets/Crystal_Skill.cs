using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;

    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;

    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();

    public override void useSkill()
    {
        base.useSkill();

        if (CanUseMultiCtystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                FindClosestEnemy(currentCrystal.transform));
        } else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    private bool CanUseMultiCtystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalList.Count > 0)
            {
                if (crystalList.Count == amountOfStacks)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                coolDown = 0;
                GameObject crystalToRespawn = crystalList[crystalList.Count - 1];
                GameObject newCrtstal = Instantiate(crystalToRespawn, player.transform.position, Quaternion.identity);

                crystalList.Remove(crystalToRespawn);
                newCrtstal.GetComponent<Crystal_Skill_Controller>()
                    .SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                        FindClosestEnemy(newCrtstal.transform));
                if (crystalList.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    RefilCrystal();
                }
            }

            return true;
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalList.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (coolDownTimer > 0)
        {
            return;
        }

        coolDownTimer = multiStackCooldown;
        RefilCrystal();
    }
}