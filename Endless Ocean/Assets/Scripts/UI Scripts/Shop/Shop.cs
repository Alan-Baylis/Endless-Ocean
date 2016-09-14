using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //List of slots in inventory.
    public List<GameObject> slots = new List<GameObject>();
    public List<Item> items = new List<Item>();
    //Vars for positioning boxes in the inventory.
    private float currentXlocation = -125;
    private float currentYLocation = 130;
    private static int INCREMENT = 62;
    private const int SHOP_BOX_SIZE = 20;
    private int slotsX, slotsY;

    //Reference to inventory slot prefab.
    public GameObject slot;

    //Rerference to tooltip.
    public GameObject toolTip;

    //List holding items user owns.
    public List<Item> inventory = new List<Item>();

    //Boolean determining when the inventory should be shown.
    private bool showShop;

    void Start()
    {
        this.initializeShop();
    }

    /// <summary>
    /// Instantiates key game objects for storing items.
    /// </summary>
    public void initializeShop()
    {
        int slotCount = 0;
        for (slotsX = 0; slotsX < 4; slotsX++)
        {
            for (int slotsY = 0; slotsY < 5; slotsY++)
            {
                items.Add(new Item());
                //Creating slot object in inventory.
                GameObject instantiatedSlot = Instantiate(slot);
                //Giving slot a name and number.
                instantiatedSlot.name = "Slot " + slotCount;
                instantiatedSlot.GetComponent<ShopSlot>().slotNumber = slotCount;
                instantiatedSlot.GetComponent<ShopSlot>().inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
                //Making slot child of parent canvas.
                instantiatedSlot.transform.parent = this.gameObject.transform;
                //Positioning slot.
                instantiatedSlot.GetComponent<RectTransform>().localPosition = new Vector3(this.currentXlocation, this.currentYLocation, 0);
                this.currentXlocation += Shop.INCREMENT;
                //Adding slot to inventory array.
                this.slots.Add(instantiatedSlot);
                slotCount++;
            }
            this.currentYLocation -= Shop.INCREMENT;
            this.currentXlocation -= (Shop.INCREMENT * 5);
        }
    }

    /// <summary>
    /// This function shows the tool tip.
    /// </summary>
    /// <param name="item">The item to show the tooltip for.</param>
    public void showToolTip(Item item)
    {
        this.toolTip.transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        this.toolTip.transform.GetChild(1).GetComponent<Text>().text = item.description;
        this.toolTip.transform.GetChild(2).GetComponent<Image>().sprite = item.itemIcon;
        this.toolTip.SetActive(true);
    }

    /// <summary>
    /// This function hides the tool tip.
    /// </summary>
    public void hideToolTip()
    {
        this.toolTip.SetActive(false);
    }

}
