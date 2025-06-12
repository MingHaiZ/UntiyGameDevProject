using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckpointId;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        } else
        {
            instance = this;
        }
    }

    private void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value)
                {
                    checkPoint.ActivateCheckpoint();
                }
            }
        }

        closestCheckpointId = _data.closestCheckpointId;
        Invoke(nameof(PlacePlayerAtClosestCheckpoint), .1f);
    }

    private void PlacePlayerAtClosestCheckpoint()
    {
        foreach (var checkPoint in checkPoints)
        {
            if (closestCheckpointId == checkPoint.id)
            {
                PlayerManager.instance.player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckpointId = FindClosestCheckPoint().id;
        _data.checkpoints.Clear();

        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.checkpoints.Add(checkPoint.id, checkPoint.activationStatus);
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position,
                checkPoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkPoint.activationStatus)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }

        return closestCheckpoint;
    }
}