using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeslot unlockCloneInsteadButton;

    public bool cloneInsteadOfCrystal { get; private set; }

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeslot unlockCrystalButton;

    public bool crystalUnlocked { get; private set; }

    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeslot unlockExplosiveButton;

    public bool canExplode { get; private set; }

    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeslot unlockMovingCrystalButton;

    public bool canMoveToEnemy { get; private set; }

    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UI_SkillTreeslot unlockMultiStackButton;

    public bool canUseMultiStacks { get; private set; }

    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosion);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMultiStack);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMovingToEnemy);
        
    }

    private void UnlockCrystal() => crystalUnlocked = unlockCrystalButton.unlocked;
    private void UnlockCrystalMirage() => cloneInsteadOfCrystal = unlockCloneInsteadButton.unlocked;
    private void UnlockCrystalExplosion() => canExplode = unlockExplosiveButton.unlocked;
    private void UnlockCrystalMultiStack() => canUseMultiStacks = unlockMultiStackButton.unlocked;
    private void UnlockCrystalMovingToEnemy() => canMoveToEnemy = unlockMovingCrystalButton.unlocked;

    public override void useSkill()
    {
        base.useSkill();

        if (CanUseMultiCtystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        } else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            } else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
            FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() =>
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();


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

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockCrystalExplosion();
        UnlockCrystalMovingToEnemy();
        UnlockCrystalMultiStack();
    }
}