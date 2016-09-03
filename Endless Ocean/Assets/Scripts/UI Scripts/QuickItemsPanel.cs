using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuickItemsPanel : MonoBehaviour {

    public Inventory inventory;

    public List<Consumable> quickItems = new List<Consumable>();

    private int numberOfQuickItems = 4;

    private static int INCREMENT = 62;
    private static int X_STARTING_POSITION = -93;

    // Use this for initialization
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
            instantiatedSlot.GetComponent<RectTransform>().localPosition = new Vector3(QuickItemsPanel.X_STARTING_POSITION + (i * QuickItemsPanel.INCREMENT), 0, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
