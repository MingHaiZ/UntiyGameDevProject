using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;

    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;

    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

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
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDealy(_data));
    }

    private void LoadCheckpoint(GameData _data)
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
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY),
                Quaternion.identity);
            newLostCurrency.GetComponent<lostCurrency>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDealy(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadCheckpoint(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }


    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        var findedClosestCheckPoint = FindClosestCheckPoint();
        
        if (findedClosestCheckPoint != null)
        {
            _data.closestCheckpointId = findedClosestCheckPoint.id;
        }
        
        _data.checkpoints.Clear();

        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.checkpoints.Add(checkPoint.id, checkPoint.activationStatus);
        }
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null)
        {
            return;
        }

        closestCheckpointId = _data.closestCheckpointId;

        foreach (var checkPoint in checkPoints)
        {
            if (closestCheckpointId == checkPoint.id)
            {
                player.position = checkPoint.transform.position;
            }
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkPoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkPoint.activationStatus)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }

        return closestCheckpoint;
    }
}