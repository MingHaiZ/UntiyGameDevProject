using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealth(int _damage)
    {
        base.DecreaseHealth(_damage);
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipmentType(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.Skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }


        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
        {
            totalDamage *= Mathf.RoundToInt(_multiplier);
        }

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        // if invnteroy current weapon has fire effect   
        DoMagicalDamage(_targetStats);
    }
}