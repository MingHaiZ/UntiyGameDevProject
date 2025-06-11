using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeslot dashUnlockButton;

    public bool dashUnlocked { get; private set; }


    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeslot cloneOnDashUnlockButton;

    public bool cloneOnDashUnlocked { get; private set; }


    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeslot cloneOnArrivalUnlockButton;

    public bool cloneOnArrivalUnlocked { get; private set; }


    public override void useSkill()
    {
        base.useSkill();
    }

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void UnlockDash() => dashUnlocked = dashUnlockButton.unlocked;
    private void UnlockCloneDash() => cloneOnDashUnlocked = cloneOnDashUnlockButton.unlocked;
    private void UnlockCloneOnArrival() => cloneOnArrivalUnlocked = cloneOnArrivalUnlockButton.unlocked;

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector2.zero);
        }
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneDash();
        UnlockCloneOnArrival();
        
    }
}