using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public struct weaponMount
{
    GameObject mountPoint;
    Weapon weapon;

    public GameObject MountPoint
    {
        get
        {
            return mountPoint;
        }
        set
        {
            mountPoint = value;
        }
    }
    public Weapon Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }
    public GameObject WeaponFromGameObject
    {
        set
        {
            value.transform.parent = mountPoint.transform;
            weapon = value.GetComponent<Weapon>();
        }
    }
    public bool weaponLoaded()
    {
        if (weapon == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public abstract class CharacterSuper : MonoBehaviour
{
    // Character variables involved in leveling
    public float movementSpeed;
    // Additional damage player does ontop of weapon damage
    public int attack;
    // Determines how much health the player has
    public int stamina;
    //The height the player will jump when the user makes them jump.
    public float jumpHeight;
    // Determines energy
    public int vigor;

    // Current level
    public int currentLevel;

    //modifier for how likely a character is to get good weapons - increase for reforger, decrease for lowly mobs
    public float luck = 1;

    // Highest possible health
    public int maxHealth;
    // current health
    public int health;

    #region movement variables
    //Movement Variables 

    protected bool facingRight;
    protected bool enableMove;
    #endregion

    #region mesh component variables
    //Character Mesh Components.
    new public Rigidbody rigidbody;
    protected Animator animator;

    public GameObject primaryWeaponSlot;
    public GameObject secondaryWeaponSlot;

    #endregion

    #region Jumping Variables
    //VARIABLES USED FOR JUMPING
    //Bool that indicates whether or not the gameobject is touching the ground.
    public bool onGround = true;
    //An array that contains the collision objects that the circle collides with when jumping.
    protected Collider[] groundCollisions;
    //The radius of the cirle to check for objects in the ground layer when jumping.
    protected float groundCheckRadius = 0.2f;
    //A layer mask that filters out game objects that are not in the ground layer.
    public LayerMask groundLayerMask;
    //The transform of a gameobject used to position the cicle used to determine if the game object is on the ground when jumping.
    public Transform groundCheck;

    #endregion

    #region Attacking Variables

    #region weapon mounts and equiping

    public Weapon weapon;

    // Mount where weapon is attached to
    public GameObject weaponMount;
    public weaponMount primaryMount;
    public weaponMount secondaryMount;

    public enum weaponMounts
    {
        Primary, Secondary
    };

    public weaponMounts activeWeaponType = weaponMounts.Primary;
    #endregion



    //Objects used for getting interface references.

    protected float nextMelee;

    #endregion

    #region Tools

    #endregion


    // Those objects which, upon collision, can damage this instance of character
    protected string fears = "Nothing";

    //A boolean indicating if the player is using an item that effects their movement.
    protected bool usingItem;

    //Character collider
    public Collider col;

    // Use this for initialization
    protected void Start()
    {
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
        this.facingRight = true;
        this.enableMove = true;


        // Get weapon mount location so that we can easily attach weapons to them
        secondaryMount.MountPoint = primaryWeaponSlot;
        primaryMount.MountPoint = secondaryWeaponSlot;
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    // <summary>
    // This function flips the game object when the user turns it around my moving it.
    // </summary>
    protected void turnAround()
    {
        this.facingRight = !facingRight;
        Vector3 reversescale = transform.localScale;
        reversescale.z *= -1;
        transform.localScale = reversescale;
    }

    protected void moveCharacter(float move)
    {
        //If the user if moving apply movement force to player.
        if (move != 0 && this.enableMove)
        {
            rigidbody.velocity = new Vector3(move * movementSpeed, this.rigidbody.velocity.y, 0);
        }

        //If the game object starts moving left and is facing right turn the object around.
        if (move > 0 && !facingRight)
        {
            this.turnAround();
        }
        //If the game object starts moving right and is facing left turn the object around.
        if (move < 0 && facingRight)
        {
            this.turnAround();
        }
        animator.SetFloat("Speed", Mathf.Abs(move));
    }


    /// <summary>
    /// Function that updates the onGround variable.
    /// </summary>
    protected void checkIfOnGround()
    {
        groundCollisions = Physics.OverlapSphere(this.groundCheck.position, this.groundCheckRadius, this.groundLayerMask);
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
    /// What happens when character collides with certain objects
    /// </summary>
    /// <param name="col">GameObject involved in collision</param>
    protected void OnTriggerEnter(Collider col)
    {
        Animator animController = col.transform.root.GetComponent<Animator>();
        int attackState = Animator.StringToHash("Attack Layer.Melee Attack");

        // When character collides DeathFromfalling gameObject (fall down hole)
        if (col.gameObject.tag == "DeathCollider")
        {
            health = 0;
        }
        // If the thing hitting the character is a projectile
        else if (col.gameObject.tag == fears+"Projectile")
        {
            int damage = col.gameObject.GetComponent<Bullet>().getDamage();
            int knockBack = col.gameObject.GetComponent<Bullet>().getKnockBack();

            this.takeDamage(damage, col.gameObject.GetComponentInParent<Rigidbody>().position, knockBack);
            Destroy(col.gameObject.GetComponentInParent<Rigidbody>().gameObject);
        }
        // When character is hit with an enemy weapon
        else if (col.gameObject.tag == fears+"Weapon")
        {
            int damage = col.gameObject.GetComponent<Weapon>().getDamage() + col.transform.root.GetComponent<CharacterSuper>().attack;
            int knockBack = col.gameObject.GetComponent<Weapon>().getKnockBack();
            bool collisionHandled = col.gameObject.GetComponent<Weapon>().collisonHandled;


            if (animController.GetCurrentAnimatorStateInfo(1).IsName("Melee Attack") && !collisionHandled)
            {
                this.takeDamage(damage, col.gameObject.GetComponentInParent<Rigidbody>().position, knockBack);
                col.gameObject.GetComponent<Weapon>().collisonHandled = true;
            }

        }
        updateHealthBar();
    }

    protected abstract void updateHealthBar();
    public abstract void die();

    /// <summary>
    /// Cause character to take damage and apply knockback
    /// </summary>
    /// <param name="damage">Amount of damage taken</param>
    /// <param name="source">Position/direction of the damage source</param>
    /// <param name="knockBack">Amount of knock back stored within damage gameObject</param>
    protected void takeDamage(int damage, Vector3 source, int knockBack)
    {
        this.health -= damage;

        //Debug.Log("I took "+damage+" damage, now my health is "+this.health +"out of a possible "+maxHealth);

        Vector3 direction = transform.position - source;
        
        direction.Normalize();
        direction.y += 0.4f;

        this.rigidbody.AddForce(direction * knockBack);

        if (this.health <= 0)
        {
            die();
        }
    }

    /// <summary>
    /// Used to equip enemies and player with weapons
    /// </summary>
    /// <param name="modelPath">Model path to gameObject weapon prefab</param>
    /// <param name="mount">Mount point for weapon to be attached (slot1 or slot2)</param>
    /// <param name="tag">Tag for weapon, determines whether enemy or player is equiping</param>
    protected void equipWeapon(string modelPath, weaponMounts mount, string tag)
    {
        switch (mount)
        {
            case weaponMounts.Primary:
                primaryMount.WeaponFromGameObject = Instantiate(Resources.Load(modelPath), weaponMount.transform.position, weaponMount.transform.rotation) as GameObject;
                primaryMount.Weapon.weaponTag = tag;
                primaryMount.Weapon.tag = tag;

                break;
            case weaponMounts.Secondary:
                secondaryMount.WeaponFromGameObject = Instantiate(Resources.Load(modelPath), weaponMount.transform.position, weaponMount.transform.rotation) as GameObject;
                secondaryMount.Weapon.WeaponTag = tag;
                secondaryMount.Weapon.tag = tag;
                break;
        }
        swapWeapons();
    }

    /// <summary>
    /// Swaps the character's currently held weapon from the two possible slots
    /// </summary>
    public void swapWeapons()
    {
        // Check for empty slots
        if (!primaryMount.weaponLoaded() || !secondaryMount.weaponLoaded())
        {
            //do not switch
        }
        // No empty slots and active weapon is the first one, switch to second wep
        else if (activeWeaponType == weaponMounts.Primary)
        {
            weapon = primaryMount.Weapon; // set as new active weapons
            activeWeaponType = weaponMounts.Secondary;
            primaryMount.MountPoint.gameObject.SetActive(true); // show weapon
            secondaryMount.MountPoint.gameObject.SetActive(false); // hide weapon

        }
        // No empty slots and active weapon is the second one, switch to first wep
        else if (activeWeaponType == weaponMounts.Secondary)
        {
            weapon = secondaryMount.Weapon; // set as new active weapon
            activeWeaponType = weaponMounts.Primary;
            secondaryMount.MountPoint.gameObject.SetActive(true); // show weapon
            primaryMount.MountPoint.gameObject.SetActive(false); // hide weapon

        }
    }
}