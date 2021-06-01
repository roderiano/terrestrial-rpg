using System.Collections;
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
    [Header("Inventory Interface")]
    [SerializeField]
    private Transform inventoryCanvas;
    [SerializeField]
    private GameObject slotButton, actionButton;
    [SerializeField]
    private SlotButton chestSlotBuild, legsSlotBuild, fireGunSlotBuild;
    
    [Header("Stats Interface")]
    [SerializeField]
    private Transform armorStatsSection;
    [SerializeField]
    private Transform fireGunStatsSection;
    
    [Header("Gameplay")]
    [SerializeField]
    private GameObject itemDrop;
    

    private PlayerArmor armor;
    private PlayerWeapons weapons;
    private PlayerMovement movement;
    private PlayerAim aim;
    private List<Slot> slots = new List<Slot>();
    private Slot selectedSlot = null;

    void Start()
    {
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
        else if(Input.GetButtonDown("Drop") && inventoryCanvas.gameObject.activeInHierarchy)
        {
            DropItem();
        }
    }

    /// <summary>
    /// Drop selected slot in UI.
    /// </summary>
    public void DropItem() 
    {   
        if(!armor.GetArmorSetSlots().Contains(selectedSlot) && weapons.GetFireGunSlot() != selectedSlot)
        {
            GameObject drop = Instantiate(itemDrop, transform.position, transform.rotation);
            drop.GetComponent<ItemDrop>().item = selectedSlot.item;

            slots.Remove(selectedSlot);
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
            slots.Add(new Slot(item, amount));
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
                if(armor.GetArmorSetSlots().Contains(slot))
                    canInstantiate = false;

                if(weapons.GetFireGunSlot() == slot)
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
    private void RefreshActionButtons() 
    {
        GameObject action;
        Transform actions = inventoryCanvas.Find("Actions");    

        foreach (Transform _action in actions)
            GameObject.Destroy(_action.gameObject);


        // Instantiate custom actions
        if(selectedSlot != null && selectedSlot.item != null)
        {
            switch (selectedSlot.item.category)
            {
                case ItemCategory.Armor:
                case ItemCategory.FireGun:
                    // Equip / Unequip
                    action = Instantiate(actionButton, Vector3.zero, Quaternion.identity, actions);
                    if(weapons.GetFireGunSlot() != selectedSlot && !armor.GetArmorSetSlots().Contains(selectedSlot))
                    {
                        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Equip);

                        // Drop
                        action = Instantiate(actionButton, Vector3.zero, Quaternion.identity, actions);
                        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Drop);
                    }
                    else
                    {
                        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Unequip);
                    }   

                    break;
            }
        }

        // Instantiate default actions
        action = Instantiate(actionButton, Vector3.zero, Quaternion.identity, actions);
        action.transform.GetComponent<ActionButton>().SetAction(MenuAction.Back);
    }

    /// <summary>
    /// Refresh item detail UI.
    /// If <paramref name="slot"/> param is null, item details components has blank.
    /// </summary>
    /// <param name="slot">Slot reference to set values.</param>
    private void RefreshItemDetailComponents() 
    {
		TextMeshProUGUI nameText = inventoryCanvas.Find("ItemsPanel/ItemDetail/Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = inventoryCanvas.Find("ItemsPanel/ItemDetail/Description").GetComponent<TextMeshProUGUI>();

        nameText.SetText(selectedSlot == null ? "" : selectedSlot.item.name);
        descriptionText.SetText(selectedSlot == null ? "" : selectedSlot.item.description);
    }

    /// <summary>
    /// Use item of selected slot.
    /// Refresh all components nedeed with new interaction. 
    /// </summary>
    public void UseItem() 
    {
        Slot slotReference = null;
        switch (selectedSlot.item.category)
        {  
            case ItemCategory.FireGun:
                slotReference = weapons.GetFireGunSlot() == selectedSlot ? null : selectedSlot;
                weapons.SetFireGunSlot(slotReference);
                break;

            case ItemCategory.Armor:
                slotReference = armor.GetArmorSetSlots().Contains(selectedSlot) ? null : selectedSlot;
                armor.SetArmorSlot(slotReference, ((Armor)selectedSlot.item).armorType);
                break;
        }

        RefreshSlotButtons();
        RefreshStatsTexts();
    }

    /// <summary>
    /// Refresh stats text's in UI.
    /// </summary>
    private void RefreshStatsTexts() 
    {
        Dictionary<string, float> armorResistence = armor.GetArmorSetResistance();
        foreach(var resistence in armorResistence)
        {
            string resistenceFieldPath = char.ToUpper(resistence.Key[0]) + resistence.Key.Substring(1) + "/Value";
            armorStatsSection.Find(resistenceFieldPath).GetComponent<TextMeshProUGUI>().SetText(resistence.Value.ToString());
        }

        Dictionary<string, float> fireGunDamage = weapons.GetFireGunDamage();
        foreach(var damage in fireGunDamage)
        {
            string damageFieldPath = char.ToUpper(damage.Key[0]) + damage.Key.Substring(1) + "/Value";
            fireGunStatsSection.Find(damageFieldPath).GetComponent<TextMeshProUGUI>().SetText(damage.Value.ToString());
        }
    }

    /// <summary>
    /// Refresh diff stats text's in UI.
    /// </summary>
    private void RefreshDiffStatsTexts() 
    {
        Dictionary<string, float> armorSetResistence = armor.GetArmorSetResistance();
        foreach(var resistence in armorSetResistence)
        {
            float diff = 0f;
            if(selectedSlot != null && selectedSlot.item.category == ItemCategory.Armor)
            {               
                Slot equipedArmorSlot = armor.GetArmorSlot(((Armor)selectedSlot.item).armorType);
                if(equipedArmorSlot != null)
                {   
                    if(equipedArmorSlot == selectedSlot) 
                    {
                        diff = -(float)equipedArmorSlot.item.GetType().GetField(resistence.Key).GetValue(equipedArmorSlot.item);
                    }
                    else
                    {
                        diff += (float)selectedSlot.item.GetType().GetField(resistence.Key).GetValue(selectedSlot.item);
                        diff -= (float)equipedArmorSlot.item.GetType().GetField(resistence.Key).GetValue(equipedArmorSlot.item);
                    }
                }
                else
                {
                    diff = (float)selectedSlot.item.GetType().GetField(resistence.Key).GetValue(selectedSlot.item);
                }
            }            

            TextMeshProUGUI text = armorStatsSection.Find(char.ToUpper(resistence.Key[0]) + resistence.Key.Substring(1) + "/DiffValue").GetComponent<TextMeshProUGUI>();
            

            if(diff > 0f)
            {
                text.SetText("+" + diff.ToString());
                text.color = Color.green;
            }
            else if(diff == 0f)
            {
               text.SetText("-"); 
               text.color = Color.white;
            }
            else
            {
                text.SetText(diff.ToString());
                text.color = Color.red;
            }
        }
    }

    /// <summary>
    /// Set selected <paramref name="slot"/> in Inventory UI.
    /// </summary>
    /// <param name="slot">Slot reference to set.</param>
    public void SetSelectedSlot(Slot slot) 
    {
        selectedSlot = slot;

        RefreshItemDetailComponents();
        RefreshDiffStatsTexts();
        RefreshActionButtons();
    }

    /// <summary>
    /// Get Item when collider with ItemDrop component.
    /// </summary>
    public void OnTriggerStay(Collider collider) 
    {
        ItemDrop itemDrop = collider.gameObject.GetComponent<ItemDrop>();
        
        if(itemDrop && Input.GetButtonDown("Get"))
        {
            AddItem(itemDrop.item, 1);
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
