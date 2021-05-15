using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler
{
    private Slot slot;
    private Button button;
    private Inventory inventory;

    void Start()
    {
        button = GetComponent<Button>();
        inventory = Object.FindObjectOfType<Inventory>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.Select();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.UseItem(slot);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(eventData.selectedObject == this.gameObject)
            inventory.SetItemDetailDisplay(slot);
    }

    public void SetSlotObject(Slot slot)
    {
        this.slot = slot;
    }
}