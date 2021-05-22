using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ***********************Herdar posteriormente da classe Stats


/// <summary>
/// The main PlayerStats class.
/// Contains all methods to handle player stats.
/// </summary>
public class PlayerStats : MonoBehaviour
{   
    private Slot fireGunSlot;
    private Slot headArmorSlot, chestArmorSlot, legsArmorSlot;


    /// <summary>
    /// Get FireGun Slot.
    /// </summary>
    /// <returns>
    /// The equiped FireGun Slot.
    /// </returns>
    public Slot GetFireGunSlot() 
    {
        return fireGunSlot;
    }

    /// <summary>
    /// Set FireGun Slot.
    /// </summary>
    /// <param name="slot">Slot of FireGun to be equiped.</param>
    public void SetFireGunSlot(Slot slot) 
    {
        fireGunSlot = slot;
    }


    /// <summary>
    /// Get equiped armor Slot by <paramref name="type"/>.
    /// </summary>
    /// <param name="type">ArmorType of armor to get.</param>
    /// <returns>
    /// The equiped armor Slot by <paramref name="type"/>.
    /// </returns>
    public Slot GetArmorSlot(ArmorType type) 
    {
        switch (type)
        {
            case ArmorType.Head:
                return headArmorSlot;
            case ArmorType.Chest:
                return chestArmorSlot;
            case ArmorType.Legs:
                return legsArmorSlot;
            default:
                return null;
        }
    }


    /// <summary>
    /// Set Armor Slot.
    /// </summary>
    /// <param name="item">Slot to be equiped.</param>
    /// <param name="type">ArmorType of armor to be equiped.</param>
    public void SetArmorSlot(Slot slot, ArmorType type) 
    {
        switch (type)
        {
            case ArmorType.Head:
                headArmorSlot = slot;
                break;
            case ArmorType.Chest:
                chestArmorSlot = slot;
                break;
            case ArmorType.Legs:
                legsArmorSlot = slot;
                break;
        }
    }

    /// <summary>
    /// Get armor set Slots.
    /// </summary>
    /// <returns>
    /// The list of Slot`s equiped.
    /// </returns>
    public List<Slot> GetArmorSetSlots() 
    {
        List<Slot> armorList = new List<Slot>();
        if(headArmorSlot != null)
            armorList.Add(headArmorSlot);
        if(chestArmorSlot != null)
            armorList.Add(chestArmorSlot);
        if(legsArmorSlot != null)
            armorList.Add(legsArmorSlot);

        return armorList;
    }
}
