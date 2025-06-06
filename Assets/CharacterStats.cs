using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    // 每点力量增加1点攻击伤害和1%的暴击伤害
    public Stat strength;

    // 每点敏捷增加1%闪避概率和1%暴击概率
    public Stat agility;

    // 每点智力增加一点魔法伤害和3点魔法抗性
    public Stat intelligence;

    // 每点活力属性增加3-5点生命值
    public Stat vitality;

    [Header("Defencive stats")]
    public Stat maxHealth;

    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;

    public Stat iceDamage;
    public Stat lightingDamage;

    [Header("Offensive stats")]
    public Stat damage;

    public Stat critChance;
    public Stat critPower;

    // 造成持续伤害
    public bool isIgnited;

    // 减少20%护甲
    public bool isChill;

    // 减少20%命中率
    public bool isShocked;

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ingniedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int igniteDamage;

    public int currentHealth;

    public System.Action OnHealthChanged;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    protected void Update()
    {
        CheckAilment();
    }

    private void CheckAilment()
    {
        CheckIgnite();
        CheckChill();
        CheckShock();
    }

    private void CheckShock()
    {
        if (isShocked)
        {
            shockedTimer -= Time.deltaTime;
            if (shockedTimer < 0)
            {
                isShocked = false;
            }
        }
    }

    private void CheckChill()
    {
        if (isChill)
        {
            chilledTimer -= Time.deltaTime;
            if (chilledTimer < 0)
            {
                isChill = false;
            }
        }
    }

    private void CheckIgnite()
    {
        if (isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            ignitedDamageTimer -= Time.deltaTime;

            if (ignitedTimer < 0)
            {
                isIgnited = false;
            }

            if (ignitedDamageTimer < 0)
            {
                DecreaseHealth(igniteDamage);
                if (currentHealth <= 0)
                {
                    Die();
                }

                ignitedDamageTimer = ingniedDamageCooldown;
            }
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }


        int totalDamage = damage.GetValue() + strength.GetValue();


        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        // _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChill)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        } else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _fireDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.25f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.33f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChill || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill)
        {
            isChill = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage,ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentsDuration;
            fx.ShockFxFor(ailmentsDuration);
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public virtual void TakeDamage(int damage)
    {
        DecreaseHealth(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;
        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) / 100f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }

    protected virtual void Die()
    {
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}