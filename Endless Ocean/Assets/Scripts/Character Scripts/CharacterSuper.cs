using UnityEngine;
using System.Collections;

public class CharacterSuper : MonoBehaviour {

    //Movement Variables 
    public float movementSpeed;
    protected bool facingRight;

    //Character Mesh Components.
    protected Rigidbody rigidbody;
    protected Animator animator;

    #region Jumping Variables
    //VARIABLES USED FOR JUMPING
    //Bool that indicates whether or not the gameobject is touching the ground.
    protected bool onGround = true;
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
    protected int health;
    protected int attackSpeed;
    protected int energy;

    //Objects used for getting interface references.
    public GameObject weaponObject;
    #endregion

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
        animator.SetFloat("Speed", Mathf.Abs(move));
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
}