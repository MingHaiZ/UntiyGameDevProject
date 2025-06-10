using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeslot parryUnlockButton;

    public bool parryUnlocked { get; private set; }

    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeslot restoreUnlockButton;

    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;

    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeslot parryWithMirageUnlockButton;

    public bool parryWithMirageUnlocked { get; private set; }

    public override void useSkill()
    {
        base.useSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthAmount);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    private void UnlockParry() => parryUnlocked = parryUnlockButton.unlocked;
    private void UnlockParryRestore() => restoreUnlocked = restoreUnlockButton.unlocked;
    private void UnlockParryWithMirage() => parryWithMirageUnlocked = parryWithMirageUnlockButton.unlocked;

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
        }
    }
}