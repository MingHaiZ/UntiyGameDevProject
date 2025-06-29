using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    [FormerlySerializedAs("checkpointId")] public string id;
    [FormerlySerializedAs("activated")] public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (!activationStatus)
        {
            AudioManager.instance.PlaySFX(5, transform);
        }
        activationStatus = true;
        anim.SetBool("active", true);
    }
}