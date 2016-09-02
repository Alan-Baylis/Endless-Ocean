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
    private GameObject itemsMenu;



    // Use this for initialization
    new void Start()
    {
        base.Start();

        base.fears = "EnemyWeapon";

        // Set health
        this.health = 100;
        this.maxHealth = 100;

        // Set energy
        this.energy = 100;
        this.maxEnergy = 100;


        this.nextMelee = 0.0f;
        this.attackSpeed = 0.2f;

        this.itemsMenu = GameObject.FindGameObjectWithTag("ItemsMenu");
        this.itemsMenu.SetActive(false);
        //Hide Menu at start.
        //COMMENTED THIS OUT FOR NOW, WAS BREAKING GAME
        //this.itemsMenu.SetActive(false);

        //this.utilityItem = this.utilityItem.GetComponent<UtilityItem>();

        this.weapon = this.weaponObject.GetComponentInChildren<Club>();

        //this.playerGrapple = this.AddComponent<Grapple>();
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
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
        if(base.energy == 0 && base.penaltyTimer < base.pentaltyLength)
        {
            base.penaltyTimer += 1;
        }
        else if (base.energy < 100)
        {
            base.penaltyTimer = 0;
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


        if (Input.GetButtonDown("OpenItemsMenu"))
        {
            itemsMenu.SetActive(!itemsMenu.activeInHierarchy);
        }
        if (Input.GetAxis("Fire 1") > 0 && nextMelee < Time.time)
        {
            nextMelee = Time.time + attackSpeed;
            if (energy > 0)
            {
                if ( energy - weapon.energyCost < 0)
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
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            //If the user if moving apply movement force to player.
            if (horizontalMove != 0)
            {
                rigidbody.velocity = new Vector3(horizontalMove * movementSpeed, this.rigidbody.velocity.y, 0);
            }

            //If the game object starts moving left and is facing right turn the object around.
            if (horizontalMove > 0 && !facingRight)
            {
                this.turnAround();
            }
            //If the game object starts moving right and is facing left turn the object around.
            if (horizontalMove < 0 && facingRight)
            {
                this.turnAround();
            }
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
}
