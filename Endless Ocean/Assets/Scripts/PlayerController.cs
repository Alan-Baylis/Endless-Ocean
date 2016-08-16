﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    



    //Movement Variables 
    public float movementSpeed;
    bool facingRight;

    //Player Components.
    Rigidbody rigidbody;
    Animator animator;
    //Other game objects.
    public Camera playerCamera;
    public GameObject weaponObject;


    #region Equiped Items
    //EQUIPPED ITEMS
     
    #endregion

    #region Grappling Variables
    //VARIABLES USED FOR GRAPPLING
    //A layermask that filters out terrain the player cannot grapple to when check to see if they are able to grapple.
    public LayerMask grappleMask;
    //The force the player can grapple with.
    public float grappleForce;
    //Bool used to tell if the user isGrappling
    private bool isGrappling;
    //Configurable joint used to act as a rope.
    private ConfigurableJoint grappleJoint;
    //Length of the rope used as the linear limit for the grappling joint.
    private float ropeLength;
    //CONSTANTS USED FOR GRAPPLING
    //Controls forces while grappling.
    private const float LINEAR_LIMIT_SPRING = 100;
    private const float LINEAR_LIMIT_DAMPER = 100;
    //A line renderer for rendering the rope the player swings on.
    private LineRenderer ropeRenderer;
    //A constant modifying the speed at which the player grapples up and down the rope.
    private const float ROPE_CHANGE_SPEED_MODIFIER = .02f;
    //A constant that is the max speed the player can move on the rope.
    private const float MAX_GRAPPLING_SPEED = 3f;
    #endregion

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
    public IWeapon weapon;

    #endregion

    // Use this for initialization
    void Start () {
        this.weapon = this.weaponObject.GetComponent<IWeapon>();
        //Retrieving components from the game objects this script is attatched to.
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.ropeRenderer = this.GetComponent<LineRenderer>();
        this.animator = this.GetComponent<Animator>();
        this.facingRight = true;  
        //Initializing grappling vars.
        this.ropeLength = 7;
        this.isGrappling = false;
    }
    
    // Update is called once per frame
    void Update ()
    {
	    
	}

    /// <summary>
    /// Runs before every frame. Performs physics calculates for game objects to be displayed when the next frame is rendered and updates the animator.
    /// </summary>
    void FixedUpdate()
    {
        if(Input.GetAxis("Fire 1") > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 mouseLocationInWorldCoords = playerCamera.ScreenToWorldPoint(mousePosition);
            this.weapon.attack(this.attack, mouseLocationInWorldCoords);
        }
        //CODE FOR GRAPPLING
        if (isGrappling)
        {
            if (Input.GetAxis("Cancel Grapple") > 0)
            {
                this.cancelGrapple();
            }
            this.swingGrapple();
        }
        //IF NOT GRAPPLING
        else if (!isGrappling) {
            if (Input.GetAxis("Grapple") > 0)
            {
                this.fireGrapple();
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
            float move = Input.GetAxis("Horizontal");
            animator.SetFloat("Speed",  Mathf.Abs(move));
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


    #region Grappling Functions
    /// <summary>
    /// This function fires the players grapple at the land and starts the player grappling if they hit a grapple-able surface.
    /// </summary>
    private void fireGrapple()
    {
        //Finding mouse position to see where use aimed the grapple.
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 mouseLocationInWorldCoords = playerCamera.ScreenToWorldPoint(mousePosition);
        RaycastHit raycastHitData = new RaycastHit();
        //Firing a raycast to see if the user can grapple on anything.
        bool canGrapple = Physics.Raycast(this.transform.position, mouseLocationInWorldCoords - this.transform.position, out raycastHitData, 7f, grappleMask);
        if (canGrapple)
        {
            //Destroy old grapple joint.
            Destroy(this.grappleJoint);
            //If the user can grapple creaing the joint they can grapple with.
            this.grappleJoint = this.gameObject.AddComponent<ConfigurableJoint>();
            this.grappleJoint.autoConfigureConnectedAnchor = false;
            this.grappleJoint.xMotion = ConfigurableJointMotion.Limited;
            this.grappleJoint.yMotion = ConfigurableJointMotion.Limited;
            this.grappleJoint.axis = new Vector3(0, 0, 1);
            //Creating a joint to act as a rope at the point of impact.
            grappleJoint.connectedBody = raycastHitData.rigidbody;
            grappleJoint.connectedAnchor = raycastHitData.rigidbody.gameObject.transform.InverseTransformPoint(raycastHitData.point);
            this.ropeRenderer.SetPositions(new Vector3[] { this.rigidbody.position, raycastHitData.point });
            this.ropeRenderer.enabled = true;
            //Allow the two objects attatched to each other to collide - prevents them from passing through eachother.
            grappleJoint.enableCollision = true;
            //Limiting the distance between the objects on the joint.
            SoftJointLimit grappleJointLimit = new SoftJointLimit();
            this.ropeLength = (raycastHitData.point - this.rigidbody.position).magnitude;
            grappleJointLimit.limit = ropeLength;
            grappleJoint.linearLimit = grappleJointLimit;
            //Adding forces that make the rope look natural when pulling the player back into the bounds.
            SoftJointLimitSpring grappleJointLimitSpring = new SoftJointLimitSpring();
            grappleJointLimitSpring.spring = PlayerController.LINEAR_LIMIT_SPRING;
            grappleJointLimitSpring.damper = PlayerController.LINEAR_LIMIT_DAMPER;
            this.grappleJoint.linearLimitSpring = grappleJointLimitSpring;
            this.isGrappling = true;
        }
    }

    /// <summary>
    /// This function swings the player on the grapple based on their horizontal input once they are swinging.
    /// </summary>
    public void swingGrapple()
    {
        //Getting horizontal movement from the user.
        float horizontalMove = Input.GetAxis("Horizontal");
        //animator.SetFloat("speed",  Mathf.Abs(move));
        //If the user if moving apply movement force to player.
        if (horizontalMove != 0)
        {

            if (this.rigidbody.velocity.magnitude < PlayerController.MAX_GRAPPLING_SPEED)
            {
                rigidbody.AddForce(new Vector3(horizontalMove * grappleForce, this.rigidbody.velocity.y, 0));
                Mathf.Clamp(this.rigidbody.velocity.magnitude, 0f, PlayerController.MAX_GRAPPLING_SPEED);
            }
        }
        float verticalMove = Input.GetAxis("Vertical");
        checkIfOnGround();
        if (verticalMove > 0 || (verticalMove < 0 && !this.onGround))
        {
            SoftJointLimit grappleJointLimit = new SoftJointLimit();
            this.ropeLength = this.ropeLength - (verticalMove * PlayerController.ROPE_CHANGE_SPEED_MODIFIER);
            this.ropeLength = Mathf.Clamp(this.ropeLength, 0f, 7f);
            grappleJointLimit.limit = ropeLength;
            grappleJoint.linearLimit = grappleJointLimit;
            this.rigidbody.AddForce(new Vector3(0, verticalMove, 0));
        }
        this.ropeRenderer.SetPosition(0, this.rigidbody.position);
    }


    /// <summary>
    /// This function stops the player grappling.
    /// </summary>
    public void cancelGrapple()
    {
        Destroy(this.grappleJoint);
        this.isGrappling = false;
        this.ropeRenderer.enabled = false;
    } 
    #endregion


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
