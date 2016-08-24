using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    //Vars for positioning labels in the inventory.
    private const float ITEM_LABEL_WIDTH = 200f;
    private const float ITEM_LABEL_HEIGHT = 50f;
    private const float ITEM_LABEL_X_POSITION = 10f;
    private const float ITEM_LABEL_Y_INCREMENT = 20f;

    private const int INVENTORY_BOX_SIZE = 20;

    private int slotsX, slotsY = 5;

    //List holding items user owns.
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();

    //Boolean determining when the inventory should be shown.
    private bool showInventory;

    void Start()
    {
        this.inventory.AddRange(this.gameObject.GetComponentsInChildren<Item>());
    }

    void Update()
    {
        if (Input.GetButtonDown("OpenInventory"))
        {
            //Toggle showing the inventory.
            this.showInventory = !showInventory;
        }
    }

    void OnGUI()
    {
        if (this.showInventory)
        {
            this.drawInventory();
        }
    }

    private void drawInventory()
    {
        for(int x = 0; x < 5; x++)
        {
            Debug.Log("X");
            for (int y = 0; y < 5; y++)
            {
                Debug.Log("Y");
                GUI.Box(new Rect(Inventory.INVENTORY_BOX_SIZE * 20, Inventory.INVENTORY_BOX_SIZE * 20, Inventory.INVENTORY_BOX_SIZE, Inventory.INVENTORY_BOX_SIZE), y.ToString());
            }
        }
    }
}
