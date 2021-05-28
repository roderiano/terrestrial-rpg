﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// The main Inventory class.
/// Contains all methods for handle inventory data and UI.
/// </summary>
public class Inventory : MonoBehaviour
{
    [Header("User Interface")]
    [SerializeField]
    private Transform inventoryCanvas;
    [SerializeField]
    private GameObject slotButton, actionButton;
    [SerializeField]
    private SlotButton chestSlotBuild, legsSlotBuild, fireGunSlotBuild;

    private PlayerStats stats;
    private PlayerArmor armor;
    private PlayerWeapons weapons;
    private PlayerMovement movement;
    private PlayerAim aim;
    private List<Slot> slots = new List<Slot>();

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        armor = GetComponent<PlayerArmor>();
        weapons = GetComponent<PlayerWeapons>();
        movement = GetComponent<PlayerMovement>();
        aim = GetComponent<PlayerAim>();
    }

    void Update()
    {
        // Enable and disable inventory UI.
        if(Input.GetButtonDown("Inventory"))
        {
            aim.SetActive(inventoryCanvas.gameObject.activeInHierarchy);
            movement.SetActive(inventoryCanvas.gameObject.activeInHierarchy);
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeInHierarchy);
            RefreshSlotButtons();
        }
    }

    /// <summary>
    /// Adds <paramref name="amount"/> or create a <c>Slot</c> for <paramref name="item"/> object.
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
    }

    /// <summary>
    /// Delete all UI SlotButton`s and instantiate a SlotButton for each inventory Slot not equiped.
    /// For equiped Slot`s in PlayerStats, their objects are set in SlotButton components.
    /// </summary>
    private void RefreshSlotButtons() 
    {
        chestSlotBuild.SetSlotObject(null);
        legsSlotBuild.SetSlotObject(null);
        fireGunSlotBuild.SetSlotObject(null);

        Transform slotsContent = inventoryCanvas.Find("ItemsPanel/SlotsView/Viewport/Content");
        foreach (Transform slot in slotsContent)
            GameObject.Destroy(slot.gameObject);

        EventSystem.current.SetSelectedGameObject(null);
        if(slots.Count > 0)
        {
            foreach(Slot slot in slots)
            {
                bool canInstantiate = true;
                if(stats.GetArmorSetSlots().Contains(slot))
                    canInstantiate = false;

                if(stats.GetFireGunSlot() == slot)
                    canInstantiate = false;

                if(canInstantiate)
                {
                    GameObject slotInstance = Instantiate(slotButton, Vector3.zero, Quaternion.identity, slotsContent);
                    SlotButton button = slotInstance.GetComponent<SlotButton>();
                    button.SetSlotObject(slot);
                    
                    if(EventSystem.current.currentSelectedGameObject == null)
                        button.Select();
                }
                else
                {
                    if(slot.item.category == ItemCategory.Armor)
                    {

                        switch(((Armor)slot.item).armorType)
                        {
                            case ArmorType.Chest:
                                chestSlotBuild.SetSlotObject(slot);
                                break;
                            case ArmorType.Legs:
                                legsSlotBuild.SetSlotObject(slot);
                                break;
                        }
                    }
                    else if(slot.item.category == ItemCategory.FireGun)
                    {
                        fireGunSlotBuild.SetSlotObject(slot);
                    }
                }
            }
        }
        else
        {
            fireGunSlotBuild.Select();
        }
    }

    /// <summary>
    /// Delete all UI ActionButton`s and instantiate the new action by <paramref name="slot"/> Item.
    /// </summary>
    /// <param name="slot">Slot reference to instantiate ActionButton`s.</param>
    public void RefreshActionButtons(Slot slot) 
    {
        GameObject action;
        Transform actions = inventoryCanvas.Find("Actions");    

        foreach (Transform _action in actions)
            GameObject.Destroy(_action.gameObject);

        // Instantiate default actions
        action = Instantiate(actionButton, Vector3.zero, Quaternion.identity, actions);
        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Back);

        // Instantiate custom actions
        if(slot != null && slot.item != null)
        {
            switch (slot.item.category)
            {
                case ItemCategory.Armor:
                case ItemCategory.FireGun:
                    action = Instantiate(actionButton, Vector3.zero, Quaternion.identity, actions);

                    if(stats.GetFireGunSlot() != slot && !stats.GetArmorSetSlots().Contains(slot))
                        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Equip);
                    else
                        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Unequip);
                    break;
            }
        }
    }

    /// <summary>
    /// Refresh item detail UI.
    /// If <paramref name="slot"/> param is null, item details components has blank.
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
    /// Use item of <paramref name="slot"/>.
    /// Refresh all components nedeed with new interaction. 
    /// </summary>
    /// <param name="slot">Slot reference to use.</param>
    public void UseItem(Slot slot) 
    {
        Slot slotReference = null;
        switch (slot.item.category)
        {  
            case ItemCategory.FireGun:
                slotReference = stats.GetFireGunSlot() == slot ? null : slot;
                stats.SetFireGunSlot(slotReference);
                weapons.EnableFireGun(slotReference);
                break;

            case ItemCategory.Armor:
                slotReference = stats.GetArmorSetSlots().Contains(slot) ? null : slot;
                stats.SetArmorSlot(slotReference, ((Armor)slot.item).armorType);
                armor.EnableArmor(slotReference, ((Armor)slot.item).armorType);
                break;
        }

        RefreshSlotButtons();
        RefreshActionButtons(slotReference);
    }

    /// <summary>
    /// Get Item when collider with ItemDrop component.
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
    public string uuid;

    public Slot(Item item, int amount) 
    {
        this.item = item;
        this.amount = amount;
        this.uuid = System.Guid.NewGuid().ToString();
    }

    public void AddAmount(int value) 
    {
        amount += value;
    }
}
