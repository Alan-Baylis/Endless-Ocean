using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This is the class for the shop. It contains functions for managing the shop slots and showing the shop tooltip.
/// </summary>
public class Shop : PanelSuper
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

    //List holding items user owns.
    public List<Item> inventory = new List<Item>();
    public Inventory inventoryGameObject;

    //Boolean determining when the inventory should be shown.
    private bool showShop;

    //Reference to reforger panel.
    public GameObject reforger;

    //Refernce to player.
    public PlayerController player;

    public AudioClip sellSound;

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
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    public override void customizeTooltip(Item item)
    {
        if (item.buyValue > player.totalTreasure) {
            this.transform.Find("Tooltip").transform.Find("Cost Label").GetComponent<Text>().color = Color.red;
        }
        else
        {
            this.transform.Find("Tooltip").transform.Find("Cost Label").GetComponent<Text>().color = Color.black;
        }
    }
}
