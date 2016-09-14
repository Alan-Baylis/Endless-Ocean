using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class SellSlot : Slot
{
    ShopPanel shop;
    PlayerController player;
    Inventory inventory;

    public void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        this.shop = GameObject.FindWithTag("Shop").GetComponent<ShopPanel>();
        this.inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        return;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.inventory.draggingItem)
        {

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
