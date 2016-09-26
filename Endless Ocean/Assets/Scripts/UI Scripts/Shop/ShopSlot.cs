using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : Slot
{
    public Shop shop;
    PlayerController player;

    public int slotNumber;
    Text itemCount;

    public void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        this.itemCount = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    /// <summary>
    /// This function runs when the user mouses over a slot.
    /// 
    /// It shows the tooltip for the item.
    /// </summary>
    /// <param name="eventData">The data from the mouse over.</param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!this.isSlotEmpty())
        {
            this.shop.showToolTip(shop.items[this.slotNumber], this.GetComponent<RectTransform>().localPosition);
        }
    }

    /// <summary>
    /// This function runs when the user mouses out of a slot.
    /// 
    /// It hides the tooltip for the item.
    /// </summary>
    /// <param name="eventData">The data from the mouse over.</param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!this.isSlotEmpty())
        {
            this.shop.hideToolTip();
        }
    }

    /// <summary>
    /// This function runs when an item is dragged.
    /// 
    /// Starts dragging the item from the slot.
    /// </summary>
    /// <param name="eventData">The data from the drag.</param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            return;
        }
        if (!this.isSlotEmpty())
        {
            this.shop.hideToolTip();
            if (this.player.totalTreasure >= this.shop.items[slotNumber].buyValue)
            {
                this.player.totalTreasure -= this.shop.items[slotNumber].buyValue;
                this.itemCount.enabled = false;
                this.inventory.startDraggingItem(shop.items[slotNumber]);
                this.shop.items[slotNumber] = new Item();
            }
        }
    }

    /// <summary>
    /// Runs when the player clicks on a slot.
    /// 
    /// Places an item in the slot if the player was dragging one.
    /// </summary>
    /// <param name="eventData">The data from the click.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            if (this.isSlotEmpty())
            {
                this.shop.items[this.slotNumber] = this.inventory.draggedItem;
                this.inventory.stopDraggingItem();
                this.player.totalTreasure += this.inventory.draggedItem.sellValue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.itemCount.enabled = false;
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.shop.items[this.slotNumber].itemIcon;
            if (this.shop.items[slotNumber].stackable)
            {
                this.itemCount.enabled = true;
                this.itemCount.text = "" + this.shop.items[this.slotNumber].itemCount;
            }
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
        try { 
            if (this.shop.items[this.slotNumber].itemName != null)
            {
                return false;
            }
            return true;
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Slot was empty. Got Exception.");
            return true;
        }
    }
}
