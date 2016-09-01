using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickItemSlot : Slot {

    private Text itemCount;
    public QuickItemsPanel quickItemsPanel;
    public int slotNumber;

    public override void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        this.quickItemsPanel = GameObject.FindWithTag("QuickItemsPanel").GetComponent<QuickItemsPanel>();
        if (this.inventory.draggingItem)
        {
            if (this.inventory.draggedItem.GetType().ToString() == "Consumable") {
                Consumable tempConsumable = (Consumable)this.inventory.draggedItem;
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
        throw new NotImplementedException();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        this.itemCount.enabled = false;
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.inventory.items[this.slotNumber].itemIcon;
            if (this.inventory.items[this.slotNumber].stackable)
            {

                this.itemCount.enabled = true;
                this.itemCount.text = "" + this.inventory.items[this.slotNumber].itemCount;
            }
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }

    // Use this for initialization
    void Start () {
        this.itemCount = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    public bool isSlotEmpty()
    {
        if (this.quickItemsPanel.quickItems[this.slotNumber].itemName != null)
        {
            return false;
        }
        return true;
    }

}
