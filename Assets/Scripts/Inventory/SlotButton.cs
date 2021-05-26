using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// The SlotButton class.
/// Contains all methods for get user event.
/// </summary>
public class SlotButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public bool buildSlot;
    
    private Slot slot;
    private Button itemButton;
    private Inventory inventory;

    void Start()
    {
        itemButton = GetComponent<Button>();
        inventory = Object.FindObjectOfType<Inventory>();
    }
    
    /// <summary>
    /// Select ItemButton on pointer enter. 
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemButton.Select();
    }

    /// <summary>
    /// Interact with Slot.
    /// </summary>
    public void Interact()
    {
        if(slot != null)
        {
            if(slot.item != null)
                inventory.UseItem(slot);
        }
    }

    /// <summary>
    /// Refresh item details components on select.
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        if(slot != null)
        {
            if(eventData.selectedObject == this.gameObject && slot.item != null)
                inventory.RefreshItemDetailComponents(slot);
        }
    }

    /// <summary>
    /// Set <paramref name="slot"/> Slot object of SlotButton and refresh their icon.
    /// </summary>
    /// <param name="slot">Slot to set in SlotButton.</param>
    public void SetSlotObject(Slot slot)
    {
        this.slot = slot;
        Image icon = transform.Find("Image").GetComponent<Image>();

        if(slot != null)
        {
            icon.enabled = true;    
            icon.sprite = slot.item.icon;  
        }
        else
        {
            icon.enabled = false;    
        }

        
    }

    /// <summary>
    /// Get Slot of object of SlotButton.
    /// </summary>
    /// <returns>
    /// The Slot of the SlotButton.
    /// </returns>
    public Slot GetSlotObject()
    {
        return slot;
    }
}