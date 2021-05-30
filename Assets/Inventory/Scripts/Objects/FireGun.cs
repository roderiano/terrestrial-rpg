
using UnityEngine;

public enum FireMode {
    Automatic,
    SemiAutomatic
}

[CreateAssetMenu(fileName="New Primary Weapon Object", menuName="Inventory/Weapons/Primary Weapon")]
public class FireGun : Weapon
{
    [Header("Configuration")]
    public FireMode fireMode;

    public void Awake()
    {
        category = ItemCategory.FireGun;
    }
}
