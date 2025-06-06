using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;

    [SerializeField] private float flashDuration = 0.2f;

    private Material originlMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;

    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originlMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = originlMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        } else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating(nameof(IgniteColorFx), 0, 0.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating(nameof(ChillColorFx), 0, 0.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating(nameof(ShockColorFx), 0, 0.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        } else
        {
            sr.color = igniteColor[1];
        }
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        } else
        {
            sr.color = shockColor[1];
        }
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        } else
        {
            sr.color = chillColor[1];
        }
    }
}