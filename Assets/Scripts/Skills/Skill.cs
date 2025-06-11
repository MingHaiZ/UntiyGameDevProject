using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    protected float coolDownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            useSkill();
            coolDownTimer = coolDown;
            return true;
        }

        return false;
    }

    public virtual void useSkill()
    {
        // do some skill spesific things
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    public bool IsCoolDown() => coolDownTimer > 0;
}