using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickItemSlot : Slot {

    private Text itemCount;
    public QuickItems quickItems;
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
        throw new NotImplementedException();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        this.itemCount = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        this.itemImage = this.GetComponentsInChildren<Image>()[1];
    }

}
