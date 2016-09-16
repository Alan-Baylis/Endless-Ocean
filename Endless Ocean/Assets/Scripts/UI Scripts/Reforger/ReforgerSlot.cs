using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReforgerSlot : Slot
{

    void Start()
    {
        this.inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

    }

    void Update()
    {
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.item.itemIcon;
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }

    /// <summary>
    /// Returns a boolean indicating whether or not the slot is empty.
    /// </summary>
    /// <returns>A boolean indicating if the slot is empty or not.</returns>
    public bool isSlotEmpty()
    {
        if (this.item.itemName != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Runs when the user clicks on the slot.
    /// 
    /// Places the dragged item in the slot.
    /// </summary>
    /// <param name="eventData">The pointer click down data.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            if (!this.isSlotEmpty())
            {
                Item tempDraggingItem = this.inventory.draggedItem;
                this.inventory.draggedItem = this.item;
                this.inventory.draggedItemIcon.GetComponent<Image>().sprite = this.item.itemIcon;
                this.item = tempDraggingItem;
            }
            else if (this.isSlotEmpty())
            {
                this.item = this.inventory.draggedItem;
                this.inventory.stopDraggingItem();
            }
        }
    }

    /// <summary>
    /// This function runs when the user drags on the slot.
    /// 
    /// It moves the object out of the reforger slot.
    /// </summary>
    /// <param name="eventData">The drag event data.</param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            return;
        }
        if (!this.isSlotEmpty())
        {
            this.inventory.hideToolTip();
            this.inventory.startDraggingItem(this.item);
            this.item = new Item();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        return;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        return;
    }
}
