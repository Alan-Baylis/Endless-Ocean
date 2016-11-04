using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : PanelSuper {

    //List of slots in inventory.
    public List<GameObject> slots = new List<GameObject>();
    public List<Item> items = new List<Item>();
    //Vars for positioning boxes in the inventory.
    private float currentXlocation = -125;
    private float currentYLocation = 130;
    private static int INCREMENT = 62;
    private const int INVENTORY_BOX_SIZE = 20;
    private int slotsX, slotsY;

    //Reference to inventory slot prefab.
    public GameObject slot;
    //Reference to dragged image.
    public Item draggedItem;
    public GameObject draggedItemIcon;
    //Reference to treasurelabel.
    public Text treasureLabel;

    //List holding items user owns.
    public List<Item> inventory = new List<Item>();

    public PlayerController player;

    //Boolean determining when the inventory should be shown.
    private bool showInventory;

    public bool draggingItem = false;

    public bool drawn;

    /// <summary>
    /// Runs when the object is first created - intializes key varaibles and positions UI elements.
    /// </summary>
    void Start()
    {
        //GameObject tp1 = Instantiate(Resources.Load("Prefabs/Consumables/TestPotion")) as GameObject;
        //this.addItem(tp1.GetComponent<Item>());
        //GameObject tp2 = Instantiate(Resources.Load("Prefabs/Consumables/TestPotion")) as GameObject;
        //this.addItem(tp2.GetComponent<Item>());
        //GameObject tp3 = Instantiate(Resources.Load("Prefabs/Consumables/TestPotion")) as GameObject;
        //this.addItem(tp3.GetComponent<Item>());
        //GameObject testHelmet = Instantiate(Resources.Load("Prefabs/Equipment/TestHelmet")) as GameObject;
        //this.addItem(testHelmet.GetComponent<Item>());
    }

    void Update()
    {
        if (draggingItem)
        {
            //Getting mouse position in inventory local space.
            Vector3 position = Input.mousePosition;
            //Offsetting mouse position so user is still able to click on inventory slots.
            draggedItemIcon.GetComponent<RectTransform>().position = new Vector3(position.x + 15, position.y - 15, position.z);
        }
        if (Input.GetButtonDown("OpenItemsMenu"))
        {
            //Toggle showing the inventory.
            this.showInventory = !showInventory;
        }
        //Update treasure label in inventory.
        this.treasureLabel.text = "$" + this.player.totalTreasure.ToString();
    }

    /// <summary>
    /// This function adds an item to the users inventory and stacks it if possible.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A boolean indicating whether or not the item was added successfully.</returns>
    public bool addItem(Item item)
    {
        if (item.stackable)
        {
            for(int i = 0; i < this.items.Count; i++)
            {
                if(this.items[i] != null && item.itemName == (this.items[i].itemName))
                {
                    stackItemInSpecifiedSlot(i, item);
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
    /// This function adds the specified item in the soonest open slot in the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A boolean indicating whether or not the item was added successfully.</returns>
    bool addItemInEmptySlot(Item item)
    {
        item.itemIcon = item.getQualityIcon();
        for (int i = 0; i < this.items.Count; i++)
        {
            if(this.items[i] == null || this.items[i].itemName == null)
            {
                addItemInSpecifiedSlot(i, item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// This functions adds an item to the stack in the specified slot.
    /// </summary>
    /// <param name="slotNumber">The slot whose count should be incremented.</param>
    public void stackItemInSpecifiedSlot(int slotNumber, Item item)
    {
            this.items[slotNumber].itemCount++;
            Destroy(item.gameObject);
    }

    /// <summary>
    /// This functions adds an item in the specified slot.
    /// </summary>
    /// <param name="slotNumber">The slot to add the item in.</param>
    public void addItemInSpecifiedSlot(int slotNumber, Item item)
    {
        this.items[slotNumber] = item;
        this.slots[slotNumber].GetComponent<InventorySlot>().item = item;
        item.gameObject.SetActive(false);
        item.gameObject.transform.parent = this.gameObject.transform;
    }

    /// <summary>
    /// This function handles adding items from other UI elements to the inventory eg from quick slot or equipment to the inventory.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    /// <param name="slotNumber">The slot to add the item in.</param>
    public void handleAddItemFromUI(Item item, int slotNumber)
    {
        if (item.stackable)
        {
            if (item.itemName == this.items[slotNumber].itemName)
            {
                this.stackItemInSpecifiedSlot(slotNumber, item);
                return;
            }
        }
        this.addItemInSpecifiedSlot(slotNumber, item);
    }


    /// <summary>
    /// This function shows an image by the cursor when an item is being dragged.
    /// </summary>
    /// <param name="item">The item being dragged.</param>
    public void startDraggingItem(Item item)
    {
        this.draggedItem = item;
        //Moving dragged item icon to the front.
        this.draggedItemIcon.transform.SetAsLastSibling();
        this.draggingItem = true;
        this.draggedItemIcon.SetActive(true);
        this.draggedItemIcon.GetComponent<Image>().sprite = item.itemIcon;
    }

    /// <summary>
    /// This function stops the user dragging an item and hides the image by the cursor.
    /// </summary>
    public void stopDraggingItem()
    {
        this.draggingItem = false;
        this.draggedItemIcon.SetActive(false);
    }

    /// <summary>
    /// Instantiates key game objects for storing items.
    /// </summary>
    public void drawInventory()
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
                instantiatedSlot.GetComponent<InventorySlot>().slotNumber = slotCount;
                instantiatedSlot.GetComponent<InventorySlot>().inventory = this;
                //Making slot child of parent canvas.
                instantiatedSlot.transform.parent = this.gameObject.transform;
                //Positioning slot.
                instantiatedSlot.GetComponent<RectTransform>().localPosition = new Vector3(this.currentXlocation, this.currentYLocation, 0);
                this.currentXlocation += Inventory.INCREMENT;
                //Adding slot to inventory array.
                this.slots.Add(instantiatedSlot);
                slotCount++;
            }
            this.currentYLocation -= Inventory.INCREMENT;
            this.currentXlocation -= (Inventory.INCREMENT * 5);
        }
        this.drawn = true;
    }
}
