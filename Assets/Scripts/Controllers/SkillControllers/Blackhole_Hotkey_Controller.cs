using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;

    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private Blackhole_Skill_Controller blackhole;

    public void SetupHotkey(KeyCode _myNewHotkey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackhole = _myBlackHole;

        myHotkey = _myNewHotkey;
        myText.text = myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            if (!blackhole.ContainsEnemyInList(myEnemy))
            {
                blackhole.AddEnemyToList(myEnemy);
            }

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}