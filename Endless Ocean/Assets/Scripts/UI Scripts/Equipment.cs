using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

    public Inventory inventory;

    public GameObject toolTip;

    public Dictionary<string, Item> equippedItems = new Dictionary<string, Item>(); 

	// Use this for initialization
	void Start () {
        equippedItems.Add("HEAD", new Item());
        equippedItems.Add("CHEST", new Item());
        equippedItems.Add("FEET", new Item());
    }

    // Update is called once per frame
    void Update () {
	
	}
}
