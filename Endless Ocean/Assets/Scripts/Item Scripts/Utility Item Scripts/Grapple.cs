using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is the class for the grapple . It is a utility item that has two modes. (Two modes is not typical of utility items.)
/// 
/// 1. It can be used to swing from certain platforms. This mode does override the players movement.
/// 
/// 2. it cam be used to pull objects towards the player. This does not affect the players movement all.
/// 
/// 
/// The idea for a rope that uses a series of hinge joints came from this http://wiki.unity3d.com/index.php?title=LineRenderer_Rope
/// </summary>
public class Grapple: MonoBehaviour
{

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
    //A reference to the other object the grapple is attatched to.
    private Rigidbody otherObject;
    //Vars and const for creating the pulling rope.
    private float ropeLength;
    private const float LINEAR_LIMIT_SPRING = 100f;
    private const float LINEAR_LIMIT_DAMPER = 100f;

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

    //Vars for drawing rope.

    private Vector3 otherEnd;


    private const float OBJECT_PULL_SPEED = .1f;
    MoveTowardsObjectGradually otherObjectMover;

    public Rigidbody playerRigidbody;
    public CameraController playerCameraController;

    // Use this for initialization
    void Start()
    {
        Grapple.ropeLineRenderer = this.gameObject.AddComponent<LineRenderer>();
        Grapple.ropeLineRenderer.SetWidth(.02f, .02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("UseUtilityItem"))
        {
            if (!grappling)
            {
                RaycastHit raycastHitData;
                bool canGrapple = Physics.Raycast(playerRigidbody.position, playerCameraController.getMouseLocationInWorldCoordinates() - playerRigidbody.position, out raycastHitData, 300f, grappleMask);
                Debug.DrawRay(playerRigidbody.position, playerCameraController.getMouseLocationInWorldCoordinates() - playerRigidbody.position);
                if (canGrapple) {
                    this.createGrapplingRope(playerCameraController.getMouseLocationInWorldCoordinates(), raycastHitData);
                }
            }
            if (!pulling)
            {
                RaycastHit raycastHitData = new RaycastHit();
                bool canPull = Physics.Raycast(playerRigidbody.position, playerCameraController.getMouseLocationInWorldCoordinates() - playerRigidbody.position, out raycastHitData, 7f, pullMask);
                if (canPull)
                {
                    this.creatingPullingRope(playerRigidbody, raycastHitData.rigidbody, raycastHitData.point);
                }
            }
        }
        else if(Input.GetButtonDown("StopUsingUtilityItem"))
        {
            this.destroyRope();
        }
    }

    void FixedUpdate()
    {
        //Overriding player movement if rope is active.
        if (grappling)
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");
            this.moveWhileGrappling(horizontalMove, verticalMove, playerRigidbody.gameObject.GetComponent<PlayerController>().onGround, playerRigidbody);
        }
    }

    void LateUpdate()
    {
        if (this.grappling)
        {
            this.drawRope(playerRigidbody.position, this.otherEnd);
        }
        else if (this.pulling)
        {
            this.drawRope(playerRigidbody.position, this.otherObject.position);
        }
    }

    //HERLPER FUNCTIONS
    #region Grappling Rope Functions
    /// <summary>
    /// This functions builds the rope between the player and the opbject they are attatched to.
    /// </summary>
    /// <param name="mouseLocationInWorldCoords"></param>
    private void createGrapplingRope(Vector3 mouseLocationInWorldCoords, RaycastHit raycastHitData)
    {
        //If the user can grapple creaing the joint they can grapple with.
        this.grappleJoint = this.createConfigurableJoint(playerRigidbody, raycastHitData.rigidbody, raycastHitData.point);
        this.otherEnd = raycastHitData.point;
        this.drawRope(playerRigidbody.position, this.otherEnd);
        this.otherObject = raycastHitData.rigidbody;
        this.grappling = true;
    }

    /// <summary>
    /// Creates a configurable joint on the specified rigidbody with the shared attributes between the to uses of the grapple.
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
        else if (this.pulling)
        {
            if (Vector3.Distance(this.otherObject.position, this.playerRigidbody.position) < 5)
            {

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
        this.otherObjectMover = secondRigidbody.gameObject.AddComponent<MoveTowardsObjectGradually>();
        otherObjectMover.init(firstRigidbody.gameObject, Grapple.OBJECT_PULL_SPEED, false, 6);
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