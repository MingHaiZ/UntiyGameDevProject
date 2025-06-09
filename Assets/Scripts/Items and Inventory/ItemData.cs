using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;
    public string itemName;
    [FormerlySerializedAs("icon")] public Sprite itemIcon;

    [Range(0f, 100f)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}