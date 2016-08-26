using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    //List of slots in inventory.
    public List<GameObject> slots = new List<GameObject>();
    public List<Item> items = new List<Item>();
    //Vars for positioning boxes in the inventory.
    private float currentXlocation = -125;
    private float currentYLocation = 160;
    private static int INCREMENT = 62;


    private const int INVENTORY_BOX_SIZE = 20;

    private int slotsX, slotsY;

    public GameObject slot;

    //List holding items user owns.
    public List<Item> inventory = new List<Item>();

    //Boolean determining when the inventory should be shown.
    private bool showInventory;

    /// <summary>
    /// Runs when the object is first created - intializes key varaibles and positions UI elements.
    /// </summary>
    void Start()
    {
        int slotCount = 0;
        for (slotsX = 0; slotsX < 4; slotsX++)
        {
            for (int slotsY = 0; slotsY < 5; slotsY++)
            {
                //Creating slot object in inventory.
                GameObject instantiatedSlot = Instantiate(slot);
                //Giving slot a name and number.
                instantiatedSlot.name = "Slot " + slotCount;
                instantiatedSlot.GetComponent<InventorySlot>().slotNumber = slotCount;
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

        this.addItem((Item)Instantiate(Resources.Load("Prefabs/Weapons/Pistol")));
    }

    void Update()
    {
        if (Input.GetButtonDown("OpenInventory"))
        {

            //Toggle showing the inventory.
            this.showInventory = !showInventory;
        }
    }   

    /// <summary>
    /// This function adds the specified item inthe soonest open slot in the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A boolean indicating whether or not the item was added successfully.</returns>
    bool addItem(Item item)
    {
        for(int i =0; i < this.items.Count; i++)
        {
            if(this.items[i].itemName == null)
            {
                Debug.Log("Added");
                this.items[i] = item;
                item.enabled = false;
                item.gameObject.transform.parent = this.gameObject.transform;
                return true;
            }
        }
        return false;
    }
}
