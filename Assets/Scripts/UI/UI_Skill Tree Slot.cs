using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_SkillTreeslot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;

    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color unLockedSkillColor;
    public bool unlocked;

    // 需要解锁的前置技能
    [SerializeField] private UI_SkillTreeslot[] shouldBeUnlocked;

    // 类似于分支选择,解锁技能时判断确保分支的技能是未解锁状态
    [SerializeField] private UI_SkillTreeslot[] shouldBelocked;


    public void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = unLockedSkillColor;
    }

    public void UnlockSkillSlot()
    {
        if (unlocked)
        {
            return;
        }

        if (!PlayerManager.instance.HaveEngouthMoney(skillPrice))
        {
            return;
        }

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
        Vector2 mousePosition = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;
        if (mousePosition.x > 600)
        {
            xOffset = -150;
        } else
        {
            xOffset = 150;
        }

        if (mousePosition.y > 320)
        {
            yOffset = -150;
        } else
        {
            yOffset = 150;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}