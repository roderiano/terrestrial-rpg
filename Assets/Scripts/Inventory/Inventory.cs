﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform inventoryCanvas;
    [SerializeField]
    private GameObject slotDisplay;

    private PlayerStats stats;
    private List<Slot> slots = new List<Slot>();

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void AddItem(Item item, int amount) 
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

        RefreshSlotsDisplay();
    }

    private void RefreshSlotsDisplay() 
    {
        Transform slotsContent = inventoryCanvas.Find("Slots/Viewport/Content");
        foreach (Transform slot in slotsContent)
            GameObject.Destroy(slot.gameObject);

        foreach(Slot slot in slots)
        {
            GameObject slotInstance = Instantiate(slotDisplay, Vector3.zero, Quaternion.identity, slotsContent);
            slotInstance.GetComponent<Button>().onClick.AddListener(() => SetItemDetailDisplay(slot));
            slotInstance.GetComponent<Button>().onClick.AddListener(() => UseItem(slot));
        }
    }

    public void SetItemDetailDisplay(Slot slot) 
    {
		inventoryCanvas.Find("ItemDetail/Name").GetComponent<Text>().text = slot.item.name;
        inventoryCanvas.Find("ItemDetail/Description").GetComponent<Text>().text = slot.item.description;
    }

    public void UseItem(Slot slot) 
    {
        switch (slot.item.category)
        {  
            case ItemCategory.FireGun:
                stats.SetFireGun(slot.item);
                break;
            case ItemCategory.Armor:
                stats.SetArmor(slot.item);
                break;
        }

    }

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
