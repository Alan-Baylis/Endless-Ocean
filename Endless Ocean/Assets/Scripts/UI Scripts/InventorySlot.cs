using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler
{

    public Item item;
    private Image itemImage;
    public int slotNumber;
    public Inventory inventory;

    /// <summary>
    /// This function runs when the user clicks on the slot.
    /// </summary>
    /// <param name="eventData">The event data from the click.</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
    }

    /// <summary>
    /// This function runs when the user mouses over a slot.
    /// 
    /// It shows the tooltip for the item.
    /// </summary>
    /// <param name="eventData">The data from the mouse over.</param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(!this.isSlotEmpty())
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
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
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
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("ASDSADASDA");
        if (!this.isSlotEmpty())
        {
            this.inventory.startDraggingItem(inventory.items[slotNumber]);
            this.inventory.items[slotNumber] = new Item();
        }
    }

    /// <summary>
    /// Runs when the player clicks on a slot.
    /// </summary>
    /// <param name="eventData">The data from the click.</param>
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
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
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.inventory.items[this.slotNumber].itemIcon;
        }
        else
        {
            this.itemImage.enabled = false;
        }
    }

    public bool isSlotEmpty()
    {
        if (this.inventory.items[this.slotNumber].itemName != null)
        {
            return false;
        }
        return true;
    }
}
