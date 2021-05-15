using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// The SlotButton class.
/// Contains all methods for get user event.
/// </summary>
public class SlotButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler
{
    [SerializeField]
    private bool buildSlot;
    private Slot slot;
    private Button itemButton;
    private Inventory inventory;

    void Start()
    {
        itemButton = GetComponent<Button>();
        inventory = Object.FindObjectOfType<Inventory>();
    }
    
    /// <summary>
    /// Select ItemButton on pointer enter 
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemButton.Select();
    }

    /// <summary>
    /// Use Item on pointer click 
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(slot != null)
        {
            inventory.UseItem(slot);

            if(buildSlot)
            {
                SetSlotObject(null);
            }
        }
    }

    /// <summary>
    /// Refresh item details components on select
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        if(eventData.selectedObject == this.gameObject)
            inventory.RefreshItemDetailComponents(slot);
    }

    /// <summary>
    /// Set Slot object and refresh icon
    /// </summary>
    public void SetSlotObject(Slot slot)
    {
        this.slot = slot;
        transform.Find("Image").GetComponent<Image>().sprite = slot == null ? null : slot.item.icon;
    }
}