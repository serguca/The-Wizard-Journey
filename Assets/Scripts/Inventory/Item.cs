using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public ItemType itemType;
    
    [Header("Stack Settings")]
    public bool isStackable = true;
    public int maxStackSize = 1;
    
    [Header("Usage")]
    public bool isUsable = false;
}

public enum ItemType
{
    Key,
    Consumable,
    Weapon,
    Tool,
    Misc
}