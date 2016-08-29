using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot: Slot
{

    public Item item;
    private Image itemImage;
    public int slotNumber;
    public Inventory inventory;
    public Equipment equipment;
    public String bodypart;

    private Text itemCount;

    /// <summary>
    /// This function runs when the user clicks on the slot.
    /// </summary>
    /// <param name="eventData">The event data from the click.</param>
    public override void OnPointerClick(PointerEventData eventData)
    {
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

            this.inventory.showToolTip(inventory.items[slotNumber]);
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
            this.inventory.hideToolTip();
        }
    }

    /// <summary>
    /// This function runs when an item is dragged.
    /// </summary>
    /// <param name="eventData">The data from the drag.</param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (!this.isSlotEmpty())
        {
            this.itemCount.enabled = false;
            this.inventory.startDraggingItem(inventory.items[slotNumber]);
            this.inventory.items[slotNumber] = new Item();
        }
    }

    /// <summary>
    /// Runs when the player clicks on a slot.
    /// </summary>
    /// <param name="eventData">The data from the click.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            if (!this.isSlotEmpty())
            {
                Item tempDraggingItem = this.inventory.draggedItem;
                this.inventory.draggedItem = this.inventory.items[this.slotNumber];
                this.inventory.draggedItemIcon.GetComponent<Image>().sprite = this.inventory.items[this.slotNumber].itemIcon;
                this.inventory.items[this.slotNumber] = tempDraggingItem;
            }
            else if (this.isSlotEmpty())
            {
                this.inventory.items[this.slotNumber] = this.inventory.draggedItem;
                this.inventory.stopDraggingItem();
            }
        }
    }

    /// <summary>
    /// Runs when the Gameobject is first created. Initializes key variables.
    /// </summary>
    void Start()
    {
        this.itemCount = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        this.itemCount.enabled = false;
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            Item tempItem;
            this.equipment.equippedItems.TryGetValue(bodypart, out tempItem);
            this.itemImage.sprite = tempItem.itemIcon;
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }

    public bool isSlotEmpty()
    {
        Item tempItem;
        this.equipment.equippedItems.TryGetValue(bodypart, out tempItem);
        if (tempItem.itemName != null)
        {
            return false;
        }
        return true;
    }
}
