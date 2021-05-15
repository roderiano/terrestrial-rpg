using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The main Inventory class.
/// Contains all methods for handle inventory.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform inventoryCanvas;
    [SerializeField]
    private GameObject slotButton;

    private PlayerStats stats;
    private List<Slot> slots = new List<Slot>();

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Adds a item amount.
    /// </summary>
    /// <param name="item">Item to add.</param>
    /// <param name="amount">Item amount.</param>
    public void AddItem(Item item, int amount) 
    {   
        if(item.category == ItemCategory.Consumable)
        {
            foreach(Slot slot in slots)
            {
                if(slot.item == item)
                {
                    slot.AddAmount(amount);
                    break;
                }
            }
        }
        else
        {
            slots.Add(new Slot(item, 1));
        } 

        RefreshSlotButtons();
    }

    /// <summary>
    /// Delete all UI SlotButton and instantiate a SlotButton for each Inventory slot.
    /// </summary>
    private void RefreshSlotButtons() 
    {
        Transform slotsContent = inventoryCanvas.Find("ItemsPanel/SlotsView/Viewport/Content");
        foreach (Transform slot in slotsContent)
            GameObject.Destroy(slot.gameObject);

        foreach(Slot slot in slots)
        {
            if(!stats.GetArmorSet().Contains(slot.item))
            {
               GameObject slotInstance = Instantiate(slotButton, Vector3.zero, Quaternion.identity, slotsContent);
                slotInstance.GetComponent<SlotButton>().SetSlotObject(slot);  
            }
        }
    }

    /// <summary>
    /// Refresh item detail UI.
    /// If slot param is null item details components has blank.
    /// </summary>
    /// <param name="slot">Slot reference to set values.</param>
    public void RefreshItemDetailComponents(Slot slot) 
    {
		TextMeshProUGUI nameText = inventoryCanvas.Find("ItemsPanel/ItemDetail/Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = inventoryCanvas.Find("ItemsPanel/ItemDetail/Description").GetComponent<TextMeshProUGUI>();

        nameText.SetText(slot == null ? "" : slot.item.name);
        descriptionText.SetText(slot == null ? "" : slot.item.description);
    }

    /// <summary>
    /// Use a Item.
    /// </summary>
    /// <param name="slot">Slot reference to use.</param>
    public void UseItem(Slot slot) 
    {
        switch (slot.item.category)
        {  
            case ItemCategory.FireGun:
                stats.SetFireGun(slot.item);
                inventoryCanvas.Find("BuildPanel/Weapons/PrimaryWeaponSlotButton").GetComponent<SlotButton>().SetSlotObject(slot);
                break;
            case ItemCategory.Armor:
                switch (((Armor)slot.item).armorType)
                {
                    case ArmorType.Head:
                        if(slot.item == stats.GetArmor(ArmorType.Head))
                            stats.SetHead(null);
                        else
                            stats.SetHead(slot.item);
                        
                        break;
                    case ArmorType.Chest:
                        if(slot.item == stats.GetArmor(ArmorType.Chest))
                            stats.SetChest(null);
                        else
                            stats.SetChest(slot.item);

                        inventoryCanvas.Find("BuildPanel/Armor/ChestSlotButton").GetComponent<SlotButton>().SetSlotObject(slot);
                        break;
                    case ArmorType.Legs:
                        if(slot.item == stats.GetArmor(ArmorType.Legs))
                            stats.SetLegs(null);
                        else
                            stats.SetLegs(slot.item);
                        
                        inventoryCanvas.Find("BuildPanel/Armor/LegsSlotButton").GetComponent<SlotButton>().SetSlotObject(slot);
                        break;
                }
                break;
        }

        RefreshSlotButtons();

    }

    /// <summary>
    /// Get Item when collider with ItemDrop component enter.
    /// </summary>
    public void OnTriggerEnter(Collider collider) 
    {
        ItemDrop dropItem = collider.gameObject.GetComponent<ItemDrop>();
        if(dropItem)
        {
            AddItem(dropItem.item, 1);
            Destroy(collider.gameObject);
        }
    }
}

/// <summary>
/// The Slot class.
/// Contains the method to control the item amount.
/// </summary>
[System.Serializable]
public class Slot 
{
    public Item item;
    public int amount;

    public Slot(Item item, int amount) 
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value) 
    {
        amount += value;
    }
}
