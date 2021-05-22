using UnityEngine;

/// <summary>
/// The main Player Armor class.
/// Contains all methods to change the armor models.
/// </summary>
public class PlayerArmor : MonoBehaviour
{   
    [SerializeField]
    private Transform headArmorsRoot, chestArmorsRoot, legsArmorsRoot;

    /// <summary>
    /// Enable only armor model of <paramref name="slot"/> item by <paramref name="type"/>.
    /// </summary>
    /// <param name="slot">Slot of armor to be equiped.</param>
    /// <param name="type">ArmorType of armor to be equiped.</param>
    public void EnableArmor(Slot slot, ArmorType type)
    {
        Transform armorsRoot;
        switch(type)
        {
            case ArmorType.Head:
                armorsRoot = headArmorsRoot;
                break;
            case ArmorType.Chest:
                armorsRoot = chestArmorsRoot;
                break;
            case ArmorType.Legs:
                armorsRoot = legsArmorsRoot;
                break;
            default:
                armorsRoot = null;
                break;
        }
        
        bool active;
        foreach (Transform armor in armorsRoot)
        {
            if(slot != null)
                active = armor.name == slot.item.id ? true : false;
            else
                active = false;

            armor.gameObject.SetActive(active);
        }
    }
}
