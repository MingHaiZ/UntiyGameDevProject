using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinshTrigger();
    }
}