using UnityEngine;

public enum ArmorType {
    Head,
    Chest,
    Legs,
}

[CreateAssetMenu(fileName="New Armor Object", menuName="Inventory/Armor")]
public class Armor : Item
{
    [Header("Configuration")]
    public ArmorType armorType;
    
    [Header("Resistance")]
    public float physical;
    public float frost, fire, magical, decay;

    public void Awake()
    {
        category = ItemCategory.Armor;
    }
}
