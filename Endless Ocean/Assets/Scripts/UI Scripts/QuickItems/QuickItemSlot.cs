using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is the class for the quick item slots. The slots that the user can store their quick items in.
/// </summary>
public class QuickItemSlot : Slot
{

    private Text itemCount;
    public QuickItemsPanel quickItemsPanel;
    public int slotNumber;

    /// <summary>
    /// When the user drags an item from the quick item bar it can be dragged into the inventory if it is open.
    /// </summary>
    /// <param name="eventData">Event data for the drag.</param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            return;
        }
        if (!this.inventory.gameObject.activeInHierarchy)
        {
            return;
        }
        if (!this.isSlotEmpty())
        {
            this.itemCount.enabled = false;
            this.inventory.startDraggingItem(quickItemsPanel.quickItems[slotNumber]);
            this.quickItemsPanel.quickItems[slotNumber] = new Consumable();
        }
    }

    /// <summary>
    /// If the user is dragging an item it is enabled and placed into the quic kitem bar at the index they click on. 
    /// 
    /// If there is an item in the slot they click on the user starts dragging that item.
    /// </summary>
    /// <param name="eventData">The event data for the click.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            if (this.inventory.draggedItem.quickItemEquipable)
            {
                Consumable tempConsumable = (Consumable)this.inventory.draggedItem;
                //Enabling items on the quick item bar so their scripts can handle the itemCount and destoying themselves.s
                tempConsumable.gameObject.SetActive(true);
                tempConsumable.gameObject.GetComponent<Consumable>().enabled = true;
                if (!this.isSlotEmpty())
                {
                    this.inventory.draggedItem = this.quickItemsPanel.quickItems[this.slotNumber];
                    this.inventory.draggedItemIcon.GetComponent<Image>().sprite = this.inventory.draggedItem.itemIcon;
                    this.quickItemsPanel.quickItems[this.slotNumber] = tempConsumable;
                }
                else if (this.isSlotEmpty())
                {
                    this.quickItemsPanel.quickItems[this.slotNumber] = tempConsumable;
                    this.inventory.stopDraggingItem();
                }
            }
            else
            {

            }
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

    /// <summary>
    /// Displays the items image in the quick slot.
    /// </summary>
    void Update()
    {
        this.itemCount.enabled = false;
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.quickItemsPanel.quickItems[this.slotNumber].itemIcon;
            if (this.quickItemsPanel.quickItems[this.slotNumber].stackable)
            {

                this.itemCount.enabled = true;
                this.itemCount.text = "" + this.quickItemsPanel.quickItems[this.slotNumber].itemCount;
            }
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }

    /// <summary>
    /// Initializes key variables.
    /// </summary>
    void Start()
    {
        this.quickItemsPanel = GameObject.FindWithTag("QuickItemsPanel").GetComponent<QuickItemsPanel>();
        this.itemCount = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    /// <summary>
    /// Returns a boolean indicating whether or not the slot is empty.
    /// </summary>
    /// <returns>A boolean indicating if the slot is empty or not.</returns>
    public bool isSlotEmpty()
    {
        if (this.quickItemsPanel.quickItems[this.slotNumber].itemName != null)
        {
            return false;
        }
        return true;
    }

}
