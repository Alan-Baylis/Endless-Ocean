using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for the grapple . It is a utility item that has two modes. (Two modes is not typical of utility items.)
/// 
/// 1. It can be used to swing from certain platforms. This mode does override the players movement.
/// 
/// 2. it cam be used to pull objects towards the player. This does not affect the players movement all.
/// </summary>
public class Grapple : UtilityItem
{

    #region Grappling Variables
    //VARIABLES USED FOR GRAPPLING
    //A layermask that filters out terrain the player cannot grapple to when check to see if they are able to grapple.
    public LayerMask grappleMask;
    //A layerMask that filters out all object that cant be pulled.
    public LayerMask pullableObjectsMask;
    //The force the player can grapple with.
    public float grappleForce;
    //Bool used to tell if the user isGrappling
    public bool isGrappling = false;
    public bool isPullingObject = false;
    Rigidbody otherObject;
    //Configurable joint used to act as a rope.
    private ConfigurableJoint grappleJoint;
    //Length of the rope used as the linear limit for the grappling joint.
    private float ropeLength = 7;
    //CONSTANTS USED FOR GRAPPLING
    //Controls forces while grappling.
    private const float LINEAR_LIMIT_SPRING = 100;
    private const float LINEAR_LIMIT_DAMPER = 100;
    //A line renderer for rendering the rope the player swings on.
    private LineRenderer ropeLineRenderer;
    //A constant modifying the speed at which the player grapples up and down the rope.
    private const float ROPE_CHANGE_SPEED_MODIFIER = .02f;
    //A constant that is the max speed the player can move on the rope.
    private const float MAX_GRAPPLING_SPEED = 3f;
    #endregion

    // Use this for initialization
    void Start()
    {
        this.ropeLineRenderer = this.gameObject.AddComponent<LineRenderer>();
        this.ropeLineRenderer.SetWidth(.02f, .02f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function fires the players grapple at the land and starts the player grappling if they hit a grapple-able surface.
    /// </summary>
    /// <param name="mouseLocationInWorldCoords">The location of the mouse in 'in-game' co-ordinates.</param>
    /// <returns>Retusn true if the playersuccesfully stop grappling, returns false otherwise.</returns>
    override public bool useItem(Vector3 mouseLocationInWorldCoords, Rigidbody playerRigidbody)
    {
        Debug.Log("Firing");
        RaycastHit raycastHitData = new RaycastHit();
        //Firing a raycast to see if the user can grapple on anything.
        bool canGrapple = Physics.Raycast(playerRigidbody.gameObject.transform.position, mouseLocationInWorldCoords - playerRigidbody.gameObject.transform.position, out raycastHitData, 7f, grappleMask);
        if (canGrapple && !isGrappling)
        {
            //If the user can grapple creaing the joint they can grapple with.
            this.grappleJoint = this.createConfigurableJoint(playerRigidbody, raycastHitData.rigidbody, raycastHitData.point);
            Debug.Log(raycastHitData.point);
            this.addRopeEffect(playerRigidbody.position, raycastHitData.point);
            this.otherObject = raycastHitData.rigidbody;
            this.isGrappling = true;
            this.affectsPlayerMovement = true;
        }
        bool canPullObject = Physics.Raycast(playerRigidbody.gameObject.transform.position, mouseLocationInWorldCoords - playerRigidbody.gameObject.transform.position, out raycastHitData, 7f, pullableObjectsMask);
        if (canPullObject && !isPullingObject)
        {
            Debug.Log("IN here");
            this.grappleJoint = this.createConfigurableJoint(playerRigidbody, raycastHitData.rigidbody, raycastHitData.point);
            this.grappleJoint.zMotion = ConfigurableJointMotion.Limited;
            //Limiting the distance between the objects on the joint.
            SoftJointLimit grappleJointLimit = new SoftJointLimit();
            grappleJointLimit.limit = 4;
            grappleJoint.linearLimit = grappleJointLimit;
            this.addRopeEffect(playerRigidbody.position, raycastHitData.point);
            this.otherObject = raycastHitData.rigidbody;
            this.isPullingObject = true;
            this.affectsPlayerMovement = false;
        }
        if(this.isGrappling || this.isPullingObject)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// This function renders the players grapple rope and calls the movement function if necessary.
    /// </summary>
    /// <param name="horizontalMove">The horizontal input axis value.</param>
    /// <param name="verticalMove">The vertical input axis value.</param>
    /// <param name="onGround">A boolean indicating whether or no the player is on the ground.</param>
    /// <param name="playerRigidbody">The rigidbody of the player that the item might effect.</param>
    /// <returns>Always returns true as the grapple always does something every frame the player is using it.</returns>
    override public bool whileUsingItem(float horizontalMove, float verticalMove, bool onGround, Rigidbody playerRigidbody)
    {
        Debug.Log("GOING");
        if (this.isGrappling)
        {
            this.moveWithItem(horizontalMove, verticalMove, onGround, playerRigidbody);
            this.ropeLineRenderer.SetPosition(0, playerRigidbody.position);
        }
        else
        {
            this.addRopeEffect(playerRigidbody.position, otherObject.position);
        }
        return true;
    }

    /// <summary>
    /// Moves the player on the grapple if the user provides movement input.
    /// </summary>
    /// <param name="horizontalMove">The horizontal input axis value.</param>
    /// <param name="verticalMove">The vertical input axis value.</param>
    /// <param name="onGround">A boolean indicating whether or no the player is on the ground.</param>
    /// <param name="playerRigidbody">The rigidbody of the player that the item might effect.</param>
    override protected void moveWithItem(float horizontalMove, float verticalMove, bool onGround, Rigidbody playerRigidbody)
    {
        Debug.Log("In move");
        if (horizontalMove != 0)
        {

            if (playerRigidbody.velocity.magnitude < Grapple.MAX_GRAPPLING_SPEED)
            {
                playerRigidbody.AddForce(new Vector3(horizontalMove * grappleForce, playerRigidbody.velocity.y, 0));
                Mathf.Clamp(playerRigidbody.velocity.magnitude, 0f, Grapple.MAX_GRAPPLING_SPEED);
            }
        }
        if (verticalMove > 0 || (verticalMove < 0 && !onGround))
        {

            SoftJointLimit grappleJointLimit = new SoftJointLimit();
            this.ropeLength = this.ropeLength - (verticalMove * Grapple.ROPE_CHANGE_SPEED_MODIFIER);
            this.ropeLength = Mathf.Clamp(this.ropeLength, 0f, 7f);
            grappleJointLimit.limit = ropeLength;
            grappleJoint.linearLimit = grappleJointLimit;
            playerRigidbody.AddForce(new Vector3(0, verticalMove, 0));
        }
    }


    /// <summary>
    /// This function stops the player grappling.
    /// </summary>
    /// <returns>Always returns true as the player can always stop using the grapple.</returns>
    override public bool stopUsingItem()
    {
        Object.Destroy(this.grappleJoint);
        this.isGrappling = false;
        this.isPullingObject = false;
        this.ropeLineRenderer.enabled = false;
        return false;
    }



    //HERLPER FUNCTIONS
    /// <summary>
    /// Creates a configurable joint on the specified rigidbody with the shared attributes between the to uses of the grapple.
    /// </summary>
    /// <param name="firstRigidbody">The rigidbody to create the joint on.</param>
    /// <param name="secondRigidbody">The rigidbody that will be the connected anchor for the joint.</param>
    /// <param name="point">The point on the second rigibody to create the confiurable joint on.</param>
    public ConfigurableJoint createConfigurableJoint(Rigidbody firstRigidbody, Rigidbody secondRigidbody, Vector3 point)
    {
        ConfigurableJoint grappleJoint = firstRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        //Setting movement restrictions on joint.
        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.xMotion = ConfigurableJointMotion.Limited;
        grappleJoint.yMotion = ConfigurableJointMotion.Limited;
        //Adding second body.
        grappleJoint.connectedBody = secondRigidbody;
        grappleJoint.connectedAnchor = secondRigidbody.gameObject.transform.InverseTransformPoint(point);
        //Allow the two objects attatched to each other to collide - prevents them from passing through eachother.
        grappleJoint.enableCollision = true;
        //Limiting the distance between the objects on the joint.
        SoftJointLimit grappleJointLimit = new SoftJointLimit();
        this.ropeLength = (point - firstRigidbody.position).magnitude;
        grappleJointLimit.limit = ropeLength;
        grappleJoint.linearLimit = grappleJointLimit;
        //Adding forces that make the rope look natural when pulling the player back into the bounds.
        SoftJointLimitSpring grappleJointLimitSpring = new SoftJointLimitSpring();
        grappleJointLimitSpring.spring = Grapple.LINEAR_LIMIT_SPRING;
        grappleJointLimitSpring.damper = Grapple.LINEAR_LIMIT_DAMPER;
        grappleJoint.linearLimitSpring = grappleJointLimitSpring;
        //Setting axis for joint.
        grappleJoint.axis = new Vector3(0, 0, 1);
        return grappleJoint;
    }

    /// <summary>
    /// Adds a line renderer that looks like a rope between two points.
    /// </summary>
    /// <param name="firstPoint">The first point for the rope.</param>
    /// <param name="secondPoint">The second point for the rope.</param>
    public void addRopeEffect(Vector3 firstPoint, Vector3 secondPoint)
    {
        this.ropeLineRenderer.SetPositions(new Vector3[] { firstPoint, secondPoint });
        this.ropeLineRenderer.enabled = true;
    }

}