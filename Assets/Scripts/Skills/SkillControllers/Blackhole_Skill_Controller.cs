using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;

    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    public int amountOfAttacks = 4;
    public float cloneAttackCoolDown = .9f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) && targets.Count > 0)
        {
            DestroyHotkeys();
            cloneAttackReleased = true;
            canCreateHotKeys = false;
        }

        if (cloneAttackReleased)
        {
            return;
        }

        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1;
            } else
            {
                xOffset = -1;
            }

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DestroyHotkeys()
    {
        if (createdHotkey.Count <= 0)
        {
            return;
        }

        for (var i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.Log("Not enough hot keys in a keycode list");
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2),
            Quaternion.identity);

        createdHotkey.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        Blackhole_Hotkey_Controller newHotkeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Controller>();
        newHotkeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
    public bool ContainsEnemyInList(Transform _enemyTransform) => targets.Contains(_enemyTransform);
}