using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [Range(0f, 100f)]
    [SerializeField] private float chanceToLooseItems;

    [Range(0f, 100f)]
    [SerializeField] private float chanceTolLooseMaterials;

    public override void GenerateDrop()
    {
        // list of equipment
        Inventory inventory = Inventory.instance;
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipment())
        {
            if (Random.Range(0f, 100f) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0f, 100f) <= chanceTolLooseMaterials)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);
        }
    }
}