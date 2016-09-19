using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterEquipment : MonoBehaviour {

    public Inventory inventory;

    public GameObject toolTip;

    public Text validationPrompt;

    private PlayerController player;

    public Text levelLabel;
    public Text healthLabel;
    public Text damageLabel;
    public Text movementSpeedLabel;
    public Text energyLabel;
    public Text armorLabel;
    public Text pointsToAllocateLabel;
    public Text currentExperienceLabel;
    public Text experienceToNextLevelLabel;

    public Text vigorLabel;
    public Text staminaLabel;

    // Level up buttons
    public Button increaseHealth;
    public Button increaseEnergy;
    public Button increaseMovementSpeed;
    public Button increaseDamage;

    public GameObject levelUpButtons;


    public Dictionary<string, Item> equippedItems = new Dictionary<string, Item>(); 

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        this.equippedItems.Add("HEAD", new Item());
        this.equippedItems.Add("CHEST", new Item());
        this.equippedItems.Add("FEET", new Item());
        this.validationPrompt = this.transform.Find("Validation Prompt").GetComponent<Text>();

        this.levelUpButtons.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        this.levelLabel.text = player.currentLevel.ToString();
        this.damageLabel.text = player.attack.ToString();
        this.healthLabel.text = player.maxHealth.ToString();
        this.energyLabel.text = player.maxEnergy.ToString();
        this.movementSpeedLabel.text = player.movementSpeed.ToString();
        this.pointsToAllocateLabel.text = player.statPointsToAllocate.ToString();
        this.staminaLabel.text = player.stamina.ToString();
        this.vigorLabel.text = player.vigor.ToString();

        this.currentExperienceLabel.text = player.currentExperience.ToString();
        this.experienceToNextLevelLabel.text = player.experienceToLevel.ToString();

        if (player.statPointsToAllocate > 0)
        {
            this.levelUpButtons.SetActive(true);
        }
        else
        {
            this.levelUpButtons.SetActive(false);
        }
    }

    public void increasePlayerHealth()
    {
        player.stamina += 1;
        player.statPointsToAllocate -= 1;
        player.updateHealth();
    }

    public void increasePlayerEnergy()
    {
        player.vigor += 1;
        player.statPointsToAllocate -= 1;
        player.updateEnergy();
    }

    public void increasePlayerMovementSpeed()
    {
        player.movementSpeed += 1;
        player.statPointsToAllocate -= 1;
    }

    public void increasePlayerDamage()
    {
        player.attack += 1;
        player.statPointsToAllocate -= 1;
    }
}
