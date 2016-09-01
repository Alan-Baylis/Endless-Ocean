using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterEquipment : MonoBehaviour {

    public Inventory inventory;

    public GameObject toolTip;

    public Text validationPrompt;

    public Dictionary<string, Item> equippedItems = new Dictionary<string, Item>(); 

	// Use this for initialization
	void Start () {
        this.equippedItems.Add("HEAD", new Item());
        this.equippedItems.Add("CHEST", new Item());
        this.equippedItems.Add("FEET", new Item());
        this.validationPrompt = this.transform.GetChild(5).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
	
	}

}
