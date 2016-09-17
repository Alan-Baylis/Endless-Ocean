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
    public Inventory inventoryGameObject;

    //Boolean determining when the inventory should be shown.
    private bool showShop;

    //Reference to reforger panel.
    public GameObject reforger;

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
                instantiatedSlot.GetComponent<ShopSlot>().inventory = this.inventoryGameObject.GetComponent<Inventory>();
                instantiatedSlot.GetComponent<ShopSlot>().shop = this;
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
    public void showToolTip(Item item, Vector3 tooltipPosition, bool canAfford)
    {
        this.toolTip.transform.SetAsLastSibling();
        this.toolTip.transform.Find("Item Name").GetComponent<Text>().text = item.itemName;
        this.toolTip.transform.Find("Item Description").GetComponent<Text>().text = item.description;
        this.toolTip.transform.Find("Item Image").GetComponent<Image>().sprite = item.itemIcon;
        this.toolTip.GetComponent<RectTransform>().localPosition = new Vector3(tooltipPosition.x + 20, tooltipPosition.y - 20, tooltipPosition.z + 1);
        if (!canAfford)
        {
            this.toolTip.transform.Find("Cost Label").GetComponent<Text>().color = Color.red;
        }
        else
        {
            this.toolTip.transform.Find("Cost Label").GetComponent<Text>().color = Color.black;
        }
        this.toolTip.transform.Find("Cost Label").GetComponent<Text>().text = "$" + item.buyValue.ToString();
        this.toolTip.SetActive(true);
    }

    /// <summary>
    /// This function hides the tool tip.
    /// </summary>
    public void hideToolTip()
    {
        this.toolTip.SetActive(false);
    }

    /// <summary>
    /// This function adds an item to the shop and stacks it if possible.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A boolean indicating whether or not the item was added successfully.</returns>
    public bool addItem(Item item)
    {
        if (item.stackable)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (item.itemName == (this.items[i].itemName))
                {
                    this.items[i].itemCount++;
                    Destroy(item.gameObject);
                    return true;
                }
            }
            return this.addItemInEmptySlot(item);
        }
        else
        {
            return this.addItemInEmptySlot(item);
        }
    }


    /// <summary>
    /// This function adds the specified item in the soonest open slot in the shop.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A boolean indicating whether or not the item was added successfully.</returns>
    bool addItemInEmptySlot(Item item)
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            if (this.items[i].itemName == null)
            {
                this.items[i] = item;
                this.slots[i].GetComponent<ShopSlot>().item = item;
                item.gameObject.SetActive(false);
                item.gameObject.transform.parent = this.gameObject.transform;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Hides this panel and shows the reforger panel.
    /// </summary>
    public void showReforger()
    {
        this.gameObject.SetActive(false);
        this.reforger.SetActive(true);
    }
}
