using UnityEngine;
using System.Collections;

public abstract class CharacterSuper : MonoBehaviour {

    // Mount where weapon is attached to
    public GameObject weaponMount;

    //Movement Variables 
    public float movementSpeed;
    protected bool facingRight;

    //Character Mesh Components.
    new public Rigidbody rigidbody;
    protected Animator animator;

    // Those objects which, upon collision, can damage this instance of character
    protected string fears = "Nothing";

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
    //The height the player will jump when the user makes them jump.
    public float jumpHeight;
    #endregion

    #region Attacking Variables
    //VARIABLES USED FOR ATTACKING
    protected float attack;
    public Weapon weapon;

    // Stat variables
    protected int maxHealth;
    protected int health;

    //Objects used for getting interface references.
    public GameObject weaponObject;
    
    protected float nextMelee;
    
    #endregion

    #region Tools

    #endregion


    //A boolean indicating if the player is using an item that effects their movement.
    protected bool usingItem;

    //Character collider
    public Collider col;

    // Use this for initialization
    protected void Start () {
        this.weapon = this.weaponObject.GetComponent<Weapon>();
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
        this.facingRight = true;
    }
	
	// Update is called once per frame
	protected void Update () {

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
        //animator.SetFloat("Speed", Mathf.Abs(move));
        //If the user if moving apply movement force to player.
        if (move != 0)
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
        // When character collides DeathFromfalling gameObject (fall down hole)
        if (col.gameObject.tag == "DeathCollider")
        {
            health = 0;
        }
        // If the thing hitting the character is a projectile
        else if(col.gameObject.tag == "PlayerProjectile")
        {
            int damage = col.gameObject.GetComponent<Bullet>().getDamage();
            int knockBack = col.gameObject.GetComponent<Bullet>().getKnockBack();

            this.takeDamage(damage, col.gameObject.GetComponentInParent<Rigidbody>().position, knockBack);
        }
        // When character is hit with an enemy weapon
        else if (col.gameObject.tag == fears)
        {
            int damage = col.gameObject.GetComponent<Weapon>().getDamage();
            int knockBack = col.gameObject.GetComponent<Weapon>().getKnockBack();

            this.takeDamage(damage, col.gameObject.GetComponentInParent<Rigidbody>().position, knockBack);
        }
        updateHealthBar();
    }

    protected abstract void updateHealthBar();
    public abstract void die();

    protected void takeDamage(int damage, Vector3 source, int knockBack)
    {
        this.health -= damage;

        //Debug.Log("I took "+damage+" damage, now my health is "+this.health +"out of a possible "+maxHealth);

        Vector3 direction = transform.position - source;
        direction.Normalize();

        this.rigidbody.AddForce(direction * knockBack);

        if(this.health <= 0)
        {
            die();
        }
    }
}