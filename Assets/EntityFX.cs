using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;

    [SerializeField] private float flashDuration = 0.2f;

    private Material originlMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originlMat = sr.material;
    }

    private IEnumerator flashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originlMat;
    }
}