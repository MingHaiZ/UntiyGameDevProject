using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;

    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    // Start is called before the first frame update
    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoulsUI();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
        {
            SetCooldownOf(swordImage);
        }

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackHoleUnlocked)
        {
            SetCooldownOf(blackHoleImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipmentType(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskImage);
        }

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(parryImage, skills.parry.coolDown);
        CheckCooldownOf(crystalImage, skills.crystal.coolDown);
        CheckCooldownOf(swordImage, skills.sword.coolDown);
        CheckCooldownOf(blackHoleImage, skills.blackhole.coolDown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        } else
        {
            soulsAmount = PlayerManager.instance.GetCurrency();
        }

        currentSouls.text = ((int)soulsAmount).ToString("N0");
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}