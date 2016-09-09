using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerController : CharacterSuper
{

    // Player HUD elements
    public Image playerHealthBar;
    public Image playerEnergyBar;

    //Other game objects.
    public CameraController playerCameraController;

    //Interfaces that separate the player controller from weapons and utility items (eg: grapple)
    public Grapple grapple;

    public int totalExperience;
    public int totalTreasure;

    //Reference to the items menu UI element.
    public GameObject itemsMenu;
    //Reference to the quickItemsPanel.
    public QuickItemsPanel quickItemsPanel;

    // ENERGY RELATED VARIABLES
    public int energy;
    public int maxEnergy;
    // How often energy regens
    protected float energyRegenSpeed = 0.1f;
    // How much energy regens each tick
    protected int regenAmount = 2;
    // Pentalty timer for when energy reaches 0
    protected int penaltyTimer = 0;
    // When pentaltyTimer reaches this value, penalty period is over
    protected int pentaltyLength = 25;


    // Use this for initialization
    new void Start()
    {
        base.Start();

        // Assign objects that damage this character upon collision
        base.fears = "Enemy";

        // Set health
        this.health = 100;
        this.maxHealth = 100;

        // Set energy and attack variables
        this.attack = 20f;
        this.energy = 100;
        this.maxEnergy = 100;
        this.nextMelee = 0.0f;
        this.itemsMenu.SetActive(false);
        //Hide Menu at start.
        //COMMENTED THIS OUT FOR NOW, WAS BREAKING GAME
        //this.itemsMenu.SetActive(false);

        //this.utilityItem = this.utilityItem.GetComponent<UtilityItem>();

        // TEMPORARY WEAPON EQUIPMENT/SWAPPING IMPLEMENTATION (Fraser, we'll need to get together and coordinate to get this working with inventory/drop/drag)
        
        equipWeapon(Club.modelPathLocal, weaponMounts.Primary, "PlayerWeapon");
        equipWeapon(Pistol.modelPathLocal, weaponMounts.Secondary, "PlayerWeapon");

        // Set primary/active player weapon as the one stored in the first slot
        weapon = meeleMount.Weapon;

        // Hide the second slot weapon so only first slot is visible
        rangedMount.MountPoint.gameObject.SetActive(false);



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
            energy += regenAmount;
            playerEnergyBar.fillAmount = (float)energy / (float)maxEnergy;

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
        // Weapon event
        if (Input.GetAxis("Fire 1") > 0 && nextMelee < Time.time) // Button to activate: Left Mouse Click
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
}
