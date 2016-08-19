using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{




    //Movement Variables 
    public float movementSpeed;
    bool facingRight;

    //Player Components.
    Rigidbody rigidbody;
    Animator animator;
    //Other game objects.
    public Camera playerCamera;

    //OTHER ITEMS THE PLAYER PICKS UP COULD BE GAME OBJECTS WITH THE ATTATCHED SCRIPTS.
    //Objects used for getting interface references.
    public GameObject weaponObject;

    //Interfaces that separate the playe controller from weapons and utility items (eg: grapple)
    public Weapon weapon;
    public UtilityItem utilityItem;

    //A boolean indicating if the player is using an item that effects their movement.
    bool usingItem;


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
    //The height the player will jump when the user makes them jump.
    public float jumpHeight;
    #endregion

    #region Attacking Variables
    //VARIABLES USED FOR ATTACKING
    private float attack;

    #endregion

    // Use this for initialization
    void Start()
    {
        this.weapon = this.weaponObject.GetComponent<Weapon>();
        this.utilityItem = this.utilityItem.GetComponent<UtilityItem>();
        //this.playerGrapple = this.AddComponent<Grapple>();
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
        this.facingRight = true;
        this.usingItem = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Runs before every frame. Performs physics calculates for game objects to be displayed when the next frame is rendered and updates the animator.
    /// </summary>
    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        Vector3 mouseLocationInWorldCoordinates = this.getMouseLocationInWorldCoordinates();

        if (Input.GetAxis("Fire 1") > 0)
        {
            this.weapon.attack(this.attack, this.getMouseLocationInWorldCoordinates());
        }
        //CODE FOR USING UTILITY ITEM
        if (this.usingItem)
        {
            if (Input.GetAxis("Stop Using Utility Item") > 0)
            {
                //Set utility item to false if successfully able to stop.
                this.usingItem = this.utilityItem.stopUsingItem();
            }
            if (this.usingItem)
            {
                //If utility item modifies the way player moves.
                this.utilityItem.whileUsingItem(horizontalMove, verticalMove, this.onGround, this.rigidbody);
            }
        }
        //IF NOT USING ITEM
        if ((this.utilityItem.affectsPlayerMovement == false) || (this.usingItem == false))
        {
            Debug.Log(rigidbody.velocity);
            if (Input.GetAxis("Use Utility Item") > 0)
            {
                //Set using item to true if successsfully able to use it.
                this.usingItem = this.utilityItem.useItem(mouseLocationInWorldCoordinates, this.rigidbody);
                Debug.Log(usingItem);
            }
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

    // <summary>
    // This function flips the game object when the user turns it around my moving it.
    // </summary>
    void turnAround()
    {
        this.facingRight = !facingRight;
        Vector3 reversescale = transform.localScale;
        reversescale.z *= -1;
        transform.localScale = reversescale;
    }



    //HELPER FUNCTIONS
    /// <summary>
    /// This function calculates the mouses location in the games world coordinates system.
    /// </summary>
    /// <returns>The mouses location in world coordinates.</returns>
    private Vector3 getMouseLocationInWorldCoordinates()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 mouseLocationInWorldCoords = playerCamera.ScreenToWorldPoint(mousePosition);
        return mouseLocationInWorldCoords;
    }

    /// <summary>
    /// Function that updates the onGround variable.
    /// </summary>
    private void checkIfOnGround()
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
