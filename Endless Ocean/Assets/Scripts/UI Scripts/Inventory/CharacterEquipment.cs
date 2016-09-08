using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterEquipment : MonoBehaviour {

    public Inventory inventory;

    public GameObject toolTip;

    public Text validationPrompt;

    private PlayerController player;

    public Text totalExperienceLabel;
    public Text nextExperienceLabel;
    public Text healthLabel;
    public Text damageLabel;
    public Text movementSpeedLabel;
    public Text energyLabel;
    public Text armorLabel;

    public Dictionary<string, Item> equippedItems = new Dictionary<string, Item>(); 

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        this.equippedItems.Add("HEAD", new Item());
        this.equippedItems.Add("CHEST", new Item());
        this.equippedItems.Add("FEET", new Item());
        this.validationPrompt = this.transform.GetChild(5).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //this.totalExperienceLabel.text = player.totalExperience.ToString();
        this.damageLabel.text = player.attack.ToString();
        this.healthLabel.text = player.maxHealth.ToString();
        this.energyLabel.text = player.maxEnergy.ToString();
        this.movementSpeedLabel.text = player.movementSpeed.ToString();
    }

}
