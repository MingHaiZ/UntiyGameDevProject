using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeslot unlockDodgeButton;

    [SerializeField] private int evasionAmount;

    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeslot unlockMirageDodgeButton;

    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge() => dodgeMirageUnlocked = unlockMirageDodgeButton.unlocked;

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}