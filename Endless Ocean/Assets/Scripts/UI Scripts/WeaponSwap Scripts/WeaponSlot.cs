using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// This class handles functionality for slots that hold weapons on the UI. It helps equip/unequip weapons from the player using the UI.
/// </summary>
public class WeaponSlot : Slot {

    
    private CharacterWeapons weapons;

    private Text itemCount;

    public int slotNumber;

    public override void OnPointerEnter(PointerEventData eventData)
    {
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
    }

    /// <summary>
    /// This function runs when an item is dragged.
    /// 
    /// Starts dragging the item from the slot and unequips the weapon from the corresponding slot on the player.
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
            this.inventory.startDraggingItem(this.weapons.equippedWeapons[this.slotNumber]);
            this.weapons.equippedWeapons[this.slotNumber] = null;
            this.weapons.removeWeaponFromUI(this.slotNumber);
        }
    }

    /// <summary>
    /// Runs when the player clicks on a slot.
    /// 
    /// Equips the item in the slot if possible.
    /// </summary>
    /// <param name="eventData">The data from the click.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {
            if (this.inventory.draggedItem.mouseClickEquipable)
            {
                Weapon tempWeapon = (Weapon)this.inventory.draggedItem;
                if (!this.isSlotEmpty())
                {
                    this.inventory.draggedItem = this.weapons.equippedWeapons[this.slotNumber];
                    this.inventory.draggedItemIcon.GetComponent<Image>().sprite = this.inventory.draggedItem.itemIcon;
                    this.weapons.equippedWeapons[slotNumber] = tempWeapon;
                    this.weapons.equipWeaponFromUI(tempWeapon.gameObject, this.slotNumber);
                }
                else if (this.isSlotEmpty())
                {
                    this.weapons.equippedWeapons[slotNumber] = tempWeapon;
                    this.weapons.equipWeaponFromUI(tempWeapon.gameObject, this.slotNumber);
                    this.inventory.stopDraggingItem();
                }     
            }
        }
    }

    /// <summary>
    /// Runs when the Gameobject is first created. Initializes key variables.
    /// </summary>
    void Start()
    {
        this.weapons = this.transform.parent.gameObject.GetComponent<CharacterWeapons>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        //If this slot is holdin an item - show it.
        if (!this.isSlotEmpty())
        {
            this.itemImage.enabled = true;
            this.itemImage.sprite = this.weapons.equippedWeapons[this.slotNumber].itemIcon;
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
        try
        {
            if (this.weapons.equippedWeapons[this.slotNumber] != null)
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
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log("Slot was empty. Got Exception.");
            return true;
        }
    }
}
