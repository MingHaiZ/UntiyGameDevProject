using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;

    [SerializeField] private UI_SkillTreeslot dashUnlockButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;

    [SerializeField] private UI_SkillTreeslot cloneOnDashUnlockButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;

    [SerializeField] private UI_SkillTreeslot cloneOnArrivalUnlockButton;

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

    private void UnlockDash() => dashUnlocked = dashUnlockButton;
    private void UnlockCloneDash() => cloneOnDashUnlocked = dashUnlockButton;
    private void UnlockCloneOnArrival() => cloneOnArrivalUnlocked = dashUnlockButton;
    
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
}