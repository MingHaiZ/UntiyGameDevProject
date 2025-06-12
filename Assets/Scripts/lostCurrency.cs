using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lostCurrency : MonoBehaviour
{
    public int currency;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            PlayerManager.instance.currency += currency;
            Destroy(this.gameObject);
        }
    }
}