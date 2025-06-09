using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;

    [SerializeField] private TextMeshProUGUI statNameText;
    
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statName != null)
        {
            statNameText.text = statName;
        }
    }

    void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerstats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerstats != null)
        {
            statValueText.text = playerstats.GetStat(statType).GetValue().ToString();
        }
    }
}