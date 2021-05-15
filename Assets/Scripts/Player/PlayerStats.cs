using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Herdar posteriormente da classe Stats
public class PlayerStats : MonoBehaviour
{   
    //--> Getters and setters
    private Item fireGun;
    private Item headArmor, chestArmor, legsArmor;

    private PlayerWeapons playerWeapons;
    private PlayerArmor playerArmor;

    void Start()
    {
        playerWeapons = GetComponent<PlayerWeapons>();
        playerArmor = GetComponent<PlayerArmor>();
    }

    public Item GetFireGun() 
    {
        return fireGun;
    }

    public void SetFireGun(Item item) 
    {
        fireGun = item;
        playerWeapons.SetFireGun(item);
    }

    public Item GetArmor(ArmorType type) 
    {
        switch (type)
        {
            case ArmorType.Head:
                return headArmor;
            case ArmorType.Chest:
                return chestArmor;
            case ArmorType.Legs:
                return legsArmor;
            default:
                return null;
        }
    }

    public List<Item> GetArmorSet() 
    {
        List<Item> armorList = new List<Item>();
        armorList.Add(headArmor);
        armorList.Add(chestArmor);
        armorList.Add(legsArmor);

        return armorList;
    }

    public void SetHead(Item item) 
    {
        headArmor = item;
        playerArmor.SetHead(item);
    }
    
    public void SetChest(Item item) 
    {
        chestArmor = item;
        playerArmor.SetChest(item);
    }

    public void SetLegs(Item item) 
    {
        legsArmor = item;
        playerArmor.SetLegs(item);
    }
}
