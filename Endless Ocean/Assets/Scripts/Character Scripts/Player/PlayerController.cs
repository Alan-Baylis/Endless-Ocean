using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlayerController : CharacterSuper
{

    // Player HUD elements FG
    public Image playerHealthBar;
    public Image playerEnergyBar;
    // Player HUD elements BG
    public Image playerHealthBarBG;
    public Image playerEnergyBarBG;

    //Other game objects.
    public CameraController playerCameraController;

    //Interfaces that separate the player controller from weapons and utility items (eg: grapple)
    public Grapple grapple;

    public int experienceToLevel;
    public int currentExperience;

    public int totalTreasure = 0;

    //Reference to the items menu UI element.
    public GameObject itemsMenu;
    //Reference to the inventory script.
    public Inventory inventory;
    //Reference to the quickItemsPanel.
    public QuickItemsPanel quickItemsPanel;

    // ENERGY RELATED VARIABLES
    public int energy;
    public int maxEnergy;
    // How often energy regens
    protected float energyRegenSpeed = 0.1f;
    // Used to calculate how to to rengerate, higher is slower
    protected int regenAmount = 35;
    // Pentalty timer for when energy reaches 0
    protected int penaltyTimer = 0;
    // When pentaltyTimer reaches this value, penalty period is over
    protected int pentaltyLength = 25;

    //items layer mask.
    public LayerMask itemsLayerMask;

    //Array of nearby item's.
    List<Item> nearbyItems = new List<Item>();

    // For leveling
    public int statPointsToAllocate;

    public int healthBarHeight = 100;

    public int ammo;

    // dodge stuff
    int dodgeCost = 15;
    int dodgeSpeed = 25;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        // Assign objects that damage this character upon collision
        base.fears = "Enemy";

        // Set energy and attack variables
        this.attack = 5;
        this.stamina = 5;
        this.vigor = 10;

        this.currentLevel = 5;

        // Set health
        this.maxHealth = this.stamina * 10;
        this.health = this.stamina * 10;


        this.maxEnergy = this.vigor * 5;
        this.energy = this.maxEnergy;

        // Set experience values
        currentExperience = 70;
        experienceToLevel = currentLevel * 20;

        this.nextMelee = 0.0f;
        this.inventory.initializeInventory();
        this.itemsMenu.SetActive(false);
        //Hide Menu at start.
        //COMMENTED THIS OUT FOR NOW, WAS BREAKING GAME
        //this.itemsMenu.SetActive(false);

        //this.utilityItem = this.utilityItem.GetComponent<UtilityItem>();

        // TEMPORARY WEAPON EQUIPMENT/SWAPPING IMPLEMENTATION (Fraser, we'll need to get together and coordinate to get this working with inventory/drop/drag)
        
        equipWeapon(Club.modelPathLocal, weaponMounts.Primary, "PlayerWeapon");
        equipWeapon(Pistol.modelPathLocal, weaponMounts.Secondary, "PlayerWeapon");

        //set the starter weapon to set weapon
        switch (activeWeaponType)
        {
            case weaponMounts.Primary:
                weapon = primaryMount.Weapon;
                break;
            case weaponMounts.Secondary:
                weapon = secondaryMount.Weapon;
                break;
        }

        //this.playerGrapple = this.AddComponent<Grapple>();
        this.facingRight = true;
        this.usingItem = false;

        // Energy regeneration, invoke repeating method
        InvokeRepeating("RegenEnergy", energyRegenSpeed, energyRegenSpeed);
    }

    /// <summary>
    /// Used to regenerate the player's energy reserves over time
    /// </summary>
    private void RegenEnergy()
    {
        if(this.energy == 0 && this.penaltyTimer < this.pentaltyLength)
        {
            this.penaltyTimer += 1;
        }
        else if (this.energy < maxEnergy)
        {
            this.penaltyTimer = 0;
            energy += this.maxEnergy/regenAmount;
            playerEnergyBar.fillAmount = (float)energy / (float)maxEnergy;
        }
    }

    public IEnumerator DoDodge()
    {
        if (energy >= dodgeCost && onGround)
        {
            enableMove = false;
            this.animator.SetBool("IsDodging",true);
            this.energy -= dodgeCost;
            if (facingRight)
                rigidbody.velocity = new Vector3(dodgeSpeed, this.rigidbody.velocity.y, 0);
            else
                rigidbody.velocity = new Vector3(-dodgeSpeed, this.rigidbody.velocity.y, 0);

            yield return new WaitForSeconds(.5f);
            rigidbody.velocity = new Vector3(0, this.rigidbody.velocity.y, 0);
            this.animator.SetBool("IsDodging", false);
            enableMove = true;
        }
    }

    /// <summary>
    /// Runs before every frame. Performs physics calculates for game objects to be displayed when the next frame is rendered and updates the animator.
    /// </summary>
    void FixedUpdate()
    {
        if(this.health <= 0)
        {
            die();
        }
        // dodge
        if (Input.GetButtonDown("Dodge")) // Button to activate: left shift
        {
            StartCoroutine(DoDodge());
        }
        // Button events
        // Swap weapons
        if (Input.GetButtonDown("SwapWeapons")) // Button to activate: Q
        {
            this.swapWeapons();
        }
        if (Input.GetButtonDown("OpenItemsMenu")) // Button to activate: I
        {
            itemsMenu.SetActive(!itemsMenu.activeInHierarchy);
        }
        if (Input.GetButtonDown("ShowNearbyItemTooltips"))
        {
            this.showNearbyItemTooltips();
        }
        if (Input.GetButtonUp("ShowNearbyItemTooltips"))
        {
            this.hideNearbyItemTooltips();
        }
        // Weapon event
        if (Input.GetButtonDown("Fire 1"))
        { // Button to activate: Left Mouse Click
            if (Physics.CheckSphere(playerCameraController.getMouseLocationInWorldCoordinates(), .01f, itemsLayerMask))
            {
                Collider[] clickedItems = Physics.OverlapSphere(playerCameraController.getMouseLocationInWorldCoordinates(), .01f, itemsLayerMask);
                inventory.addItem(clickedItems[0].gameObject.GetComponent<Item>());

            }
            else if (nextMelee < Time.time)
            {
                nextMelee = Time.time + weapon.getAttackSpeed();
                if (energy > 0)
                {
                    if (energy - weapon.energyCost < 0)
                    {
                        energy = 0;
                    }
                    else
                    {
                        energy -= weapon.energyCost;
                    }

                    this.playerEnergyBar.fillAmount = (float)this.energy / (float)this.maxEnergy;
                    this.animator.SetTrigger("MeleeAttackTrigger");
                    this.weapon.attack(this.attack, playerCameraController.getMouseLocationInWorldCoordinates());
                }
            }
        }
        //If statements for using quick items.
        #region
        if (Input.GetButtonDown("UseQuickItem1"))
        {
            this.quickItemsPanel.useQuickItem(0);
        }
        else if (Input.GetButtonDown("UseQuickItem2"))
        {
            this.quickItemsPanel.useQuickItem(1);
        }
        else if (Input.GetButtonDown("UseQuickItem3"))
        {
            this.quickItemsPanel.useQuickItem(2);
        }
        else if (Input.GetButtonDown("UseQuickItem4"))
        {
            this.quickItemsPanel.useQuickItem(3);
        }
        #endregion
        if (!grapple.grappling || (grapple.grappling && onGround))
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");

            //IF NOT USING ITEM
            //CODE FOR JUMPING.
            if (onGround && (Input.GetAxis("Jump") > 0))
            {
                this.onGround = false;
                this.animator.SetBool("grounded", this.onGround);
                this.rigidbody.AddForce(new Vector3(0, jumpHeight, 0));
            }
            checkIfOnGround();
            this.animator.SetBool("grounded", this.onGround);

            //CODE FOR MOVING.
            moveCharacter(horizontalMove);
        }
        // Check for level up
        if(currentExperience >= experienceToLevel)
        {
            levelUp();
        }

        //Update treasure label in inventory.
        this.inventory.treasureLabel.text = "$" + this.totalTreasure.ToString();
    }

    public void levelUp()
    {
        // Level up
        statPointsToAllocate += 5;
        int leftOverExperience = currentExperience - experienceToLevel;
        currentExperience = leftOverExperience;
        currentLevel += 1;
        calculateExperienceToLevel();
    }

    /// <summary>
    /// Used to update health with new stamina on levelup
    /// </summary>
    public void updateHealth()
    {
        this.maxHealth = this.stamina * 10;
        //playerHealthBar.transform.localScale = new Vector3(2, 1, 0);
        //playerHealthBarBG.transform.localScale = new Vector3(2, 1, 0);
        this.updateHealthBar();
    }

    /// <summary>
    /// Used to update energy with new vigor on levelup
    /// </summary>
    public void updateEnergy()
    {
        this.maxEnergy = this.vigor * 5;
        //playerEnergyBar.rectTransform.sizeDelta = new Vector2(healthBarHeight, this.maxEnergy);
        //playerEnergyBarBG.rectTransform.sizeDelta = new Vector2(healthBarHeight, this.maxEnergy);
        this.updateEnergyBar();
    }

    public void calculateExperienceToLevel()
    {
        experienceToLevel = currentLevel * 20;
    }

    /// <summary>
    /// What happens when player collides with certain objects
    /// </summary>
    /// <param name="col">GameObject involved in collision</param>
    new void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        // When player is hit with an enemy weapon
        if (col.gameObject.tag == "EnemyWeapon")
        {
            Debug.Log("Player: My health is now " + this.health + "and I took " + "some" + "damage");
        }
    }

    protected override void updateHealthBar()
    {
        // Update health bar with new health
        playerHealthBar.fillAmount = (float)this.health / (float)this.maxHealth;
    }

    protected void updateEnergyBar()
    {
        // Update health bar with new health
        playerEnergyBar.fillAmount = (float)this.energy / (float)this.maxEnergy;
    }

    public override void die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    /// <summary>
    /// This functions closes all the open UI windows.
    /// </summary>
    public void closeAllUIWindows()
    {
        itemsMenu.SetActive(false);
    }

    /// <summary>
    /// This functions enables the tool tips of all the nearby items.
    /// </summary>
    public void showNearbyItemTooltips()
    {
        this.nearbyItems.Clear();
        Collider[] nearbyColliders = Physics.OverlapSphere(playerCameraController.getMouseLocationInWorldCoordinates(), 15f, itemsLayerMask);
        for(int i = 0; i < nearbyColliders.Length; i++)
        {
            nearbyColliders[i].gameObject.GetComponent<Item>().tooltip.SetActive(true);
            nearbyItems.Add(nearbyColliders[i].gameObject.GetComponent<Item>());
        }
    }

    /// <summary>
    /// This function hides all nearby item tool tips.
    /// </summary>
    public void hideNearbyItemTooltips()
    {
        for (int i = 0; i < nearbyItems.Count; i++)
        {
            nearbyItems[i].gameObject.GetComponent<Item>().tooltip.SetActive(false);
        }
    }
}
