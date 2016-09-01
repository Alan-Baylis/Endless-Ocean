using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : CharacterSuper
{
    // Player HUD elements
    public Image playerHealthBar;
    public Image playerEnergyBar;

    //Other game objects.
    public CameraController playerCameraController;

    //Interfaces that separate the player controller from weapons and utility items (eg: grapple)
    public Weapon weapon;
    public Grapple grapple;

    public int totalExperience;
    public int totalTreasure;

    //A boolean indicating if the player is using an item that effects their movement.
    bool usingItem;

    //Character collider
    public Collider col;

    //Reference to the items menu UI element.
    private GameObject itemsMenu;

    #region Jumping Variables
    //VARIABLES USED FOR JUMPING
    //Bool that indicates whether or not the gameobject is touching the ground.
    bool onGround = true;
    //An array that contains the collision objects that the circle collides with when jumping.
    Collider[] groundCollisions;
    //The radius of the cirle to check for objects in the ground layer when jumping.
    float groundCheckRadius = 0.2f;
    //A layer mask that filters out game objects that are not in the ground layer.
    public LayerMask groundLayerMask;
    //The transform of a gameobject used to position the cicle used to determine if the game object is on the ground when jumping.
    public Transform groundCheck;
    #endregion

    #region Attacking Variables
    //VARIABLES USED FOR ATTACKING
    private float attack;
    private float attackSpeed;
    private float nextMelee;

    // ENERGY RELATED VARIABLES
    private int energy;
    private int maxEnergy;
    // How often energy regens
    private float energyRegenSpeed = 0.1f;
    // How much energy regens each tick
    private int regenAmount = 2;
    // Pentalty timer for when energy reaches 0
    private int penaltyTimer = 0;
    // When pentaltyTimer reaches this value, penalty period is over
    private int pentaltyLength = 25;

    #endregion

    // Use this for initialization
    void Start()
    {
        // Set health
        this.health = 100;
        this.maxHealth = 100;
        // Set energy
        this.energy = 100;
        this.maxEnergy = 100;

        this.itemsMenu = GameObject.FindGameObjectWithTag("ItemsMenu");
        this.itemsMenu.SetActive(false);
        //Hide Menu at start.
        //COMMENTED THIS OUT FOR NOW, WAS BREAKING GAME
        //this.itemsMenu.SetActive(false);
        this.weapon = this.weaponObject.GetComponentInChildren<Club>();
        //this.playerGrapple = this.AddComponent<Grapple>();
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
        this.facingRight = true;
        this.usingItem = false;
        this.energy = 100;
        this.nextMelee = 0.0f;
        this.attackSpeed = 0.2f;

        // Energy regeneration, invoke repeating method
        InvokeRepeating("RegenEnergy", energyRegenSpeed, energyRegenSpeed);
    }

    /// <summary>
    /// Used to regenerate the player's energy reserves over time
    /// </summary>
    private void RegenEnergy()
    {
        if(this.energy == 0 && penaltyTimer < pentaltyLength)
        {
            penaltyTimer += 1;
        }
        else if (this.energy < 100)
        {
            penaltyTimer = 0;
            this.energy += regenAmount;
            this.playerEnergyBar.fillAmount = (float)this.energy / (float)this.maxEnergy;

        }
    }
    /// <summary>
    /// Runs before every frame. Performs physics calculates for game objects to be displayed when the next frame is rendered and updates the animator.
    /// </summary>
    void FixedUpdate()
    {
        if(this.health <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        if (Input.GetButtonDown("OpenItemsMenu"))
        {
            itemsMenu.SetActive(!itemsMenu.activeInHierarchy);
        }
        if (Input.GetAxis("Fire 1") > 0 && nextMelee < Time.time)
        {
            nextMelee = Time.time + attackSpeed;
            if (this.energy > 0)
            {
                if ( this.energy - weapon.energyCost < 0)
                {
                    this.energy = 0;
                }
                else
                {
                    this.energy -= weapon.energyCost;
                }

                this.playerEnergyBar.fillAmount = (float)this.energy / (float)this.maxEnergy;
                this.animator.SetTrigger("MeleeAttackTrigger");
                //this.weapon.attack(this.attack, playerCameraController.getMouseLocationInWorldCoordinates());
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
    /// This function flips the game object when the user turns it around my moving it.
    /// </summary>
    void turnAround()
    {
        this.facingRight = !facingRight;
        Vector3 reversescale = transform.localScale;
        reversescale.z *= -1;
        transform.localScale = reversescale;
    }

    /// <summary>
    /// Function that updates the onGround variable.
    /// </summary>
    private void checkIfOnGround()
    {
        groundCollisions = Physics.OverlapSphere(this.groundCheck.position, this.groundCheckRadius);
        if (groundCollisions.Length > 0)
        {
            this.onGround = true;
        }
        else
        {
            this.onGround = false;
        }
    }

    /// <summary>
    /// What happens when player collides with certain objects
    /// </summary>
    /// <param name="col">GameObject involved in collision</param>
    void OnTriggerEnter(Collider col)
    {
        // When player collides DeathFromfalling gameObject (fall down hole)
        if (col.gameObject.tag == "DeathCollider")
        {
            health = 0;
        }
        // When player is hit with an enemy weapon
        if (col.gameObject.tag == "EnemyWeapon")
        {
            // Temporary. Enemies should use weapons just like player (similar to how this is done in the enemy onTriggerEnter method
            int damage = 35;
            int knockBack = 5;
            Vector3 test = new Vector3(1, 1, 1);

            this.takeDamage(damage, test, knockBack);
            // Update health bar with new health
            playerHealthBar.fillAmount = (float)this.health / (float)this.maxHealth;

            Debug.Log("Player: My health is now " + this.health + "and I took " + damage + "damage");
        }
    }
}
