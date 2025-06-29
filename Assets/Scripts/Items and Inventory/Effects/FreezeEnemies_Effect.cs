using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class NewBehaviourScript : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        CharacterStats playerStats = PlayerManager.instance.player.stats;

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_respawnPosition.position, 2);
        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}