using UnityEngine;

public enum ArmorType {
    Head,
    Chest,
    Legs,
}

[CreateAssetMenu(fileName="New Armor Object", menuName="Inventory/Armor")]
public class Armor : Item
{
    [Header("Armor Info")]
    public ArmorType armorType;
    
    [Header("Resistence Attributes")]
    public float physicalResistence;

    public void Awake()
    {
        category = ItemCategory.Armor;
    }
}
