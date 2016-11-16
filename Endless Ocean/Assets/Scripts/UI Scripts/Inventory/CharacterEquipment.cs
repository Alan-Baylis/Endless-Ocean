using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This class handles all functionality related to equipment and leveling for the character.
/// </summary>
/// 
public enum Bodypart { HEAD, CHEST, FEET };

public class CharacterEquipment : PanelSuper
{
    //Reference to the players inventory. 
    public Inventory inventory;

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

    public Transform head;

    //Ints used to revert player stats when equipment is removed.
    private int oldHealth;
    private int oldEnergy;

    public Dictionary<Bodypart, Equipment> equippedItems = new Dictionary<Bodypart, Equipment>();

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        this.equippedItems.Add(Bodypart.HEAD, new Equipment());
        this.equippedItems.Add(Bodypart.CHEST, new Equipment());
        this.equippedItems.Add(Bodypart.FEET, new Equipment());
        this.validationPrompt = this.transform.Find("Validation Prompt").GetComponent<Text>();

        this.levelUpButtons.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        this.levelLabel.text = player.currentLevel.ToString();
        this.damageLabel.text = player.attack.ToString();
        this.healthLabel.text = player.health.ToString() +"/"+ player.maxHealth.ToString();
        this.energyLabel.text = player.energy.ToString() + "/" + player.maxEnergy.ToString();
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
        player.updateHealth(1);
    }

    public void increasePlayerEnergy()
    {
        player.vigor += 1;
        player.statPointsToAllocate -= 1;
        player.updateEnergy(1);
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

    /// <summary>
    /// This function equips the item on the specified body part.
    /// </summary>
    /// <param name="bodypart">The body part to equip the item on.</param>
    /// <param name="equipment">The item to equip.</param>
    public void equipItem(Bodypart bodypart, Equipment equipment)
    {
        this.equippedItems[bodypart] = equipment;
        if (bodypart == Bodypart.HEAD)
        {
            equipment.transform.parent = this.head;
            this.enableAndPositionItem(equipment);
            this.addEquipmentStatsToPlayer(equipment);
        }
        else if (bodypart == Bodypart.CHEST)
        {
            this.addEquipmentStatsToPlayer(equipment);
        }
        else if(bodypart == Bodypart.FEET)
        {
            this.addEquipmentStatsToPlayer(equipment);
        }
    }

    /// <summary>
    /// This function enables and positions and item on the player model. It also disables the item tooltip.
    /// </summary>
    /// <param name="equipment">The equipment to enable and position.</param>
    public void enableAndPositionItem(Equipment equipment)
    {
        equipment.GetComponent<Rigidbody>().isKinematic = true;
        equipment.gameObject.transform.localPosition = Vector3.zero;
        equipment.gameObject.SetActive(true);
        equipment.tooltip.gameObject.SetActive(false);
    }

    /// <summary>
    /// This function adds some equipments stats to the player.
    /// </summary>
    /// <param name="equipment">The equipment whose stats are being added.</param>
    public void addEquipmentStatsToPlayer(Equipment equipment)
    {
        this.player.vigor += equipment.vigorBonus;
        this.player.stamina += equipment.staminaBonus;
        this.player.movementSpeed += equipment.moveSpeedBonus;
        this.player.attack += equipment.damageBonus;

        this.player.updateEnergy(equipment.vigorBonus);
        this.player.updateHealth(equipment.staminaBonus);
    }

    /// <summary>
    /// This function removes some equipments stats from the player.
    /// </summary>
    /// <param name="equipment">The equipment whose stats are being removed.</param>
    public void removeEquipmentStatsFromPlayer(Equipment equipment)
    {
        this.player.vigor -= equipment.vigorBonus;
        this.player.stamina -= equipment.staminaBonus;
        this.player.movementSpeed -= equipment.moveSpeedBonus;
        this.player.attack -= equipment.damageBonus;

        this.player.updateEnergy(equipment.vigorBonus*-1);
        this.player.updateHealth(equipment.staminaBonus*-1);
    }

}