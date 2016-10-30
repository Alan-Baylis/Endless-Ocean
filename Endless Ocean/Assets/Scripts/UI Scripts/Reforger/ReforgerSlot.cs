using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is the class for the reforger. It handles functionality to do with re-rolling item rarity and handling user input with reforging.
/// </summary>
public class ReforgerSlot : Slot
{

    //Reference to shop.
    public GameObject shop;

    Text costLabel;
    int reforgeCost = 20;
    PlayerController player;
    public GameObject validationPrompt;
    public PanelSuper reforgerPanel;

    public AudioClip reforgeSound;

    void Start()
    {
        this.item = new Item();
        this.itemImage = this.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
        this.costLabel = GameObject.FindGameObjectWithTag("ReforgerCostLabel").GetComponent<Text>();
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            if (this.inventory.draggedItem.reforgable) {
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
            this.reforgerPanel.hideToolTip();
            this.inventory.startDraggingItem(this.item);
            this.item = new Item();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!this.isSlotEmpty())
        {
            this.reforgerPanel.showToolTip(this.item, this.GetComponent<RectTransform>().localPosition);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!this.isSlotEmpty())
        {
            this.reforgerPanel.hideToolTip();
        }
    }

    /// <summary>
    /// This function reforges the item in the reforger slot.
    /// </summary>
    public void reforgeItem()
    {
        this.validationPrompt.SetActive(false);
        if (!this.isSlotEmpty() && player.totalTreasure > reforgeCost)
        {
            Weapon tempWeapon = (Weapon)this.item;
            tempWeapon.setQuality(player.luck);
            tempWeapon.itemIcon = tempWeapon.getQualityIcon();
            this.item = tempWeapon;
            player.totalTreasure -= reforgeCost;
            AudioSource.PlayClipAtPoint(this.reforgeSound, this.transform.position);
        }
        else
        {
            this.validationPrompt.SetActive(true);
        }
    }

    /// <summary>
    /// Hides this panel and shows the shop panel.
    /// </summary>
    public void showShop()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
        this.shop.SetActive(true);
    }
}
