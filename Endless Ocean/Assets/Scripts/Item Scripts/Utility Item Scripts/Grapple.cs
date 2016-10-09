using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is the class for the grapple . It has two modes.
/// 
/// 1. It can be used to swing from certain platforms. This mode does override the players movement.
/// 
/// 2. it cam be used to pull objects towards the player. This does not affect the players movement all.
/// 
/// </summary>
public class Grapple: MonoBehaviour
{
    public bool isEnabled;

    float lastFire;
    float grappleReloadSpeed;

    //Layer masks for the uses of the grappling hook.
    public LayerMask grappleMask;
    public LayerMask pullMask;
    //Bools indicating what the grapple is doing.
    public bool grappling = false;
    public bool pulling = false;
    //The line rendere that draws the grapples rope.
    private static LineRenderer ropeLineRenderer;
    private Vector3 otherEnd;
    //A reference to the other object the grapple is attatched to.
    private Rigidbody otherObject;
    

    //Vars for the grapplong ROPE.
    private ConfigurableJoint grappleJoint;
    //A constant modifying the speed at which the player grapples up and down the rope.
    private const float ROPE_CHANGE_SPEED_MODIFIER = .1f;
    //A constant that is the max speed the player can move on the rope.
    private const float MAX_GRAPPLING_SPEED = 5f;
    //The force the player can grapple with.
    private const float GRAPPLE_FORCE = 3f;
    //Contants restraining the length of the rope.
    private const float MAX_ROPE_LENGTH = 13f;
    private const float MIN_ROPE_LENGTH = 4f;
    //Vars and const for creating the grappling rope.
    private float ropeLength;
    private const float LINEAR_LIMIT_SPRING = 100f;
    private const float LINEAR_LIMIT_DAMPER = 100f;

    //Vars for pulling rope.
    private const float OBJECT_PULL_SPEED = .1f;
    private const float MAX_PULLING_ROPE_LENGTH = 10f;
    GrapplePullObject otherObjectMover;

    public Rigidbody playerRigidbody;

    // Use this for initialization
    void Start()
    {
        Grapple.ropeLineRenderer = this.gameObject.AddComponent<LineRenderer>();
        Grapple.ropeLineRenderer.SetWidth(.02f, .02f);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1) && isEnabled)
        {
            if (this.grappling || this.pulling)
            {
                this.destroyRope();
            }
            else
            {
                RaycastHit grappleRaycastHitData;
                bool canGrapple = Physics.Raycast(playerRigidbody.position, Camera.main.GetComponent<CameraController>().getMouseLocationInWorldCoordinates() - playerRigidbody.position, out grappleRaycastHitData, 15f, grappleMask);
                if (canGrapple)
                {
                    this.createGrapplingRope(grappleRaycastHitData);
                }
                else
                {
                    RaycastHit pullingRaycastHitData = new RaycastHit();
                    bool canPull = Physics.Raycast(playerRigidbody.position, Camera.main.GetComponent<CameraController>().getMouseLocationInWorldCoordinates() - playerRigidbody.position, out pullingRaycastHitData, 7f, pullMask);
                    if (canPull)
                    {
                        this.creatingPullingRope(playerRigidbody, pullingRaycastHitData.rigidbody, pullingRaycastHitData.point);
                    }
                }
            }
        }
        else if (Input.GetButtonDown("StopUsingUtilityItem"))
        {
            this.destroyRope();
        }
    }

    void FixedUpdate()
    {
        //Overriding player movement if rope is active.
        if (this.grappling)
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");
            this.moveWhileGrappling(horizontalMove, verticalMove, playerRigidbody.gameObject.GetComponent<PlayerController>().onGround, playerRigidbody);
        }
        if (this.pulling)
        {
            this.handlePullingRopeStretching();
        }
    }

    /// <summary>
    /// This function draws the grapple's rope.
    /// </summary>
    void LateUpdate()
    {
        if (this.grappling)
        {
            this.drawRope(this.gameObject.transform.position, this.otherEnd);
        }
        else if (this.pulling)
        {
            this.drawRope(this.gameObject.transform.position, this.otherObject.position);
        }
    }

    //HERLPER FUNCTIONS
    #region Grappling Rope Functions
    /// <summary>
    /// This functions builds the rope using a configurable joint that the player can swing on.
    /// </summary>
    /// <param name="raycastHitData">The data from the raycst that stuck the object the player will grapple from.</param>
    private void createGrapplingRope(RaycastHit raycastHitData)
    {
        Destroy(this.playerRigidbody.gameObject.GetComponent<ConfigurableJoint>());
        //If the user can grapple creaing the joint they can grapple with.
        this.grappleJoint = this.createConfigurableJoint(playerRigidbody, raycastHitData.rigidbody, raycastHitData.point);
        this.otherEnd = raycastHitData.point;
        this.drawRope(playerRigidbody.position, this.otherEnd);
        this.otherObject = raycastHitData.rigidbody;
        this.grappling = true;
    }

    /// <summary>
    /// Creates a configurable joint on the specified rigidbody that the player can swing from.
    /// </summary>
    /// <param name="firstRigidbody">The rigidbody to create the joint on.</param>
    /// <param name="secondRigidbody">The rigidbody that will be the connected anchor for the joint.</param>
    /// <param name="point">The point on the second rigibody to create the confiurable joint on.</param>
    private ConfigurableJoint createConfigurableJoint(Rigidbody firstRigidbody, Rigidbody secondRigidbody, Vector3 point)
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
    /// Moves the player on the grapple if the user provides movement input.
    /// </summary>
    /// <param name="horizontalMove">The horizontal input axis value.</param>
    /// <param name="verticalMove">The vertical input axis value.</param>
    /// <param name="onGround">A boolean indicating whether or no the player is on the ground.</param>
    /// <param name="playerRigidbody">The rigidbody of the player that the item might effect.</param>
    private void moveWhileGrappling(float horizontalMove, float verticalMove, bool onGround, Rigidbody playerRigidbody)
    {
        if (this.grappling)
        {
            if (horizontalMove != 0)
            {

                if (playerRigidbody.velocity.magnitude < Grapple.MAX_GRAPPLING_SPEED)
                {
                    playerRigidbody.AddForce(new Vector3(horizontalMove * Grapple.GRAPPLE_FORCE, playerRigidbody.velocity.y, 0));
                    Mathf.Clamp(playerRigidbody.velocity.magnitude, 0f, Grapple.MAX_GRAPPLING_SPEED);
                }
            }
            if (verticalMove > 0 || (verticalMove < 0 && !onGround))
            {

                SoftJointLimit grappleJointLimit = new SoftJointLimit();
                this.ropeLength = this.ropeLength - (verticalMove * Grapple.ROPE_CHANGE_SPEED_MODIFIER);
                this.ropeLength = Mathf.Clamp(this.ropeLength, Grapple.MIN_ROPE_LENGTH, Grapple.MAX_ROPE_LENGTH);
                grappleJointLimit.limit = ropeLength;
                grappleJoint.linearLimit = grappleJointLimit;
                playerRigidbody.AddForce(new Vector3(0, verticalMove, 0));
            }
        }
    }
    #endregion

    #region Pulling Rope Functions
    /// <summary>
    /// Creates a rope used pulling an object between the player and the target object.
    /// </summary>
    /// <param name="firstRigidbody">The rigidbody to create the joint on.</param>
    /// <param name="secondRigidbody">The rigidbody that will be the connected anchor for the joint.</param>
    /// <param name="point">The point on the second rigibody to create the confiurable joint on.</param>
    public void creatingPullingRope(Rigidbody firstRigidbody, Rigidbody secondRigidbody, Vector3 point)
    {
        this.otherObjectMover = secondRigidbody.gameObject.AddComponent<GrapplePullObject>();
        otherObjectMover.init(firstRigidbody.gameObject, Grapple.OBJECT_PULL_SPEED, false, playerRigidbody);
        //ConfigurableJoint grappleJoint = firstRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        ////Setting movement restrictions on joint.
        //grappleJoint.autoConfigureConnectedAnchor = false;
        //grappleJoint.xMotion = ConfigurableJointMotion.Limited;
        //grappleJoint.yMotion = ConfigurableJointMotion.Limited;
        ////Adding second body.
        //grappleJoint.connectedBody = secondRigidbody;
        this.otherEnd = secondRigidbody.position;
        this.otherObject = secondRigidbody;
        //grappleJoint.connectedAnchor = secondRigidbody.gameObject.transform.InverseTransformPoint(point);
        ////Allow the two objects attatched to each other to collide - prevents them from passing through eachother.
        //grappleJoint.enableCollision = true;
        ////Limiting the distance between the objects on the joint.
        //SoftJointLimit grappleJointLimit = new SoftJointLimit();
        //this.ropeLength = (point - firstRigidbody.position).magnitude;
        //grappleJointLimit.limit = ropeLength;
        //grappleJoint.linearLimit = grappleJointLimit;
        ////Adding forces that make the rope look natural when pulling the player back into the bounds.
        //SoftJointLimitSpring grappleJointLimitSpring = new SoftJointLimitSpring();
        //grappleJointLimitSpring.spring = Grapple.LINEAR_LIMIT_SPRING;
        //grappleJointLimitSpring.damper = Grapple.LINEAR_LIMIT_DAMPER;
        //grappleJoint.linearLimitSpring = grappleJointLimitSpring;
        ////Setting axis for joint.
        //grappleJoint.axis = new Vector3(0, 0, 1);
        this.pulling = true;
        //return grappleJoint;
    }

    /// <summary>
    /// This function moves the player while they are puling 
    /// </summary>
    public void handlePullingRopeStretching()
    {
        if (Vector3.Distance(this.otherObject.position, this.playerRigidbody.position) > Grapple.MAX_PULLING_ROPE_LENGTH)
        {
            this.destroyRope();
        }
    }
    #endregion

    /// <summary>
    /// This function destroys all the rope game objects and resets key variables.
    /// </summary>
    public void destroyRope()
    {
        if (grappling)
        {
            ropeLineRenderer.SetVertexCount(0);
            Destroy(this.playerRigidbody.gameObject.GetComponent<ConfigurableJoint>());
            grappling = false;
        }
        if (pulling)
        {
            Destroy(this.otherObjectMover);
        }
        ropeLineRenderer.enabled = false;
        grappling = false;
        pulling = false;
    }

    /// <summary>
    /// Adds a line renderer that looks like a rope between two points.
    /// </summary>
    /// <param name="firstPoint">The first point for the rope.</param>
    /// <param name="secondPoint">The second point for the rope.</param>
    public void drawRope(Vector3 firstPoint, Vector3 secondPoint)
    {
        Grapple.ropeLineRenderer.SetVertexCount(2);
        Grapple.ropeLineRenderer.SetPositions(new Vector3[] { firstPoint, secondPoint });
        Grapple.ropeLineRenderer.enabled = true;
    }
}