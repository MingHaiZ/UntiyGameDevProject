using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0f, 100f) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count > 0)
            {
                ItemData randomItem = dropList[Random.Range(0, dropList.Count)];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }

            
        }
    }

    public void DropItem(ItemData _itemData)
    {
        
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 6), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}