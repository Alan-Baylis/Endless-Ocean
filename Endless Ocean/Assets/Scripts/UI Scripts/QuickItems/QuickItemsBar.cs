using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the class for the panel the user uses quick items from.
/// 
/// It contains references to the users quick items and has the functionality for using/removing quick items.
/// </summary>
public class QuickItemsBar : MonoBehaviour {

    public Inventory inventory;

    //Reference for the player that the consumables may affect.
    public PlayerController player;

    public List<Consumable> quickItems = new List<Consumable>() {new Consumable(), new Consumable(), new Consumable(), new Consumable()};
    public List<QuickItemSlot> slots = new List<QuickItemSlot>();

    private int numberOfQuickItems = 4;

    private static int INCREMENT = 62;
    private static int X_STARTING_POSITION = -93;

    /// <summary>
    /// Initializes key variables.
    /// </summary>
    void Start () {
        //this.inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
	    for(int i = 0; i < this.numberOfQuickItems; i++)
        {
            quickItems.Add(new Consumable());
            //Creating slot object in inventory.
            GameObject instantiatedSlot = Instantiate(Resources.Load("Prefabs/UI/QuickItemSlot") as GameObject);
            //Giving slot a name and number.
            instantiatedSlot.name = "Quick Item Slot " + i;
            instantiatedSlot.GetComponent<QuickItemSlot>().slotNumber = i;
            instantiatedSlot.GetComponent<QuickItemSlot>().inventory = inventory;

            //Making slot child of parent canvas.
            instantiatedSlot.transform.parent = this.gameObject.transform;
            instantiatedSlot.GetComponent<RectTransform>().localPosition = new Vector3(QuickItemsBar.X_STARTING_POSITION + (i * QuickItemsBar.INCREMENT), 0, 0);
            slots.Add(instantiatedSlot.GetComponent<QuickItemSlot>());
        }
	}

    /// <summary>
    /// Uses the quick item in the slot index if it exists.
    /// 
    /// Decreases the itemCount of the item by 1 and removes it form the quick item bar if it is 0.
    /// </summary>
    /// <param name="index">The index of the quick item in the quick items panel to use.</param>
    public void useQuickItem(int index)
    {
        if(!(this.quickItems[index].itemName == null))
        {
            this.quickItems[index].gameObject.SetActive(true);
            this.quickItems[index].use(this.player);
            this.quickItems[index].itemCount--;
            this.quickItems[index].gameObject.SetActive(false);
            if (this.quickItems[index].itemCount == 0)
            {
                this.quickItems[index] = new Consumable();
            }
            
        }
    }
}
