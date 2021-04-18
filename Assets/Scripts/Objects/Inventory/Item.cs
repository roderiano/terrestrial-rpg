using UnityEngine;
using UnityEditor;

public enum ItemCategory {
    FireGun,
    Consumable,
    Armor
}

public abstract class Item : ScriptableObject
{
    [Header("Item Information")]
    public string id;
    public string description;
    public float weight;

    [HideInInspector]
    public ItemCategory category;
}
