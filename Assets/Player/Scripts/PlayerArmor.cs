using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The main Player Armor class.
/// Contains all methods to change the armor models.
/// </summary>
public class PlayerArmor : MonoBehaviour
{   
    [SerializeField]
    private Transform headArmorsRoot, chestArmorsRoot, legsArmorsRoot;
    private Slot headArmorSlot, chestArmorSlot, legsArmorSlot;

    /// <summary>
    /// Enable only armor model of <paramref name="slot"/> item by <paramref name="type"/>.
    /// </summary>
    /// <param name="slot">Slot of armor to be equiped.</param>
    /// <param name="type">ArmorType of armor to be equiped.</param>
    private void EnableArmor(Slot slot, ArmorType type)
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
        
        EnableArmor(slot, type);
    }

    /// <summary>
    /// Get armor set resistances.
    /// </summary>
    /// <returns>
    /// The dictionary of armor set resistance.
    /// </returns>
    public Dictionary<string, float> GetArmorSetResistance() 
    {
        List<Slot> armorSet = GetArmorSetSlots();
        string[] resistanceKeys = {"physical", "frost", "fire", "magical", "decay"};
        Dictionary<string, float> resistanceDictionary = new Dictionary<string, float>();

        foreach (string key in resistanceKeys)
        {
            bool keyCreated = false;

            foreach (Slot slot in armorSet)
            {
                float value = (float)slot.item.GetType().GetField(key).GetValue(slot.item); 
                
                if(resistanceDictionary.ContainsKey(key))
                {
                    resistanceDictionary[key] += value;   
                }
                else
                {
                    resistanceDictionary.Add(key, value);
                    keyCreated = true;
                }
            }

            if(!keyCreated)
                resistanceDictionary.Add(key, 0f);
        }

        return resistanceDictionary;
    }
}
