using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
        //TODO add to stack
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}