using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;

    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
        {
            return;
        }

        if (triggered)
        {
            return;
        }

        transform.position =
            Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(2.5f, 2.5f);
            anim.transform.localPosition = new Vector3(0, .5f);
            triggered = true;

            Invoke(nameof(DamageAndSelfDestory), .2f);

            anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}