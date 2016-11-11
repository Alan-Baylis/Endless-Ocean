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
public class Grapple : MonoBehaviour
{
    public bool isEnabled;

    float lastFire;
    float grappleReloadSpeed;

    //Layer mask for the uses of the grappling hook.
    public LayerMask grappleMask;
    //Bools indicating what the grapple is doing.
    public bool grappling = false;
    public bool pulling = false;
    public bool retractingRope = false;
    //The line rendere that draws the grapples rope.
    private LineRenderer ropeLineRenderer;
    public Vector3 otherEnd;
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

    Coroutine retractRopeCoroutine;

    public AudioClip grappleShotNoise;
    public AudioClip grappleRetractNoise;

    /// <summary>
    /// Initializes key variables.
    /// </summary>
    void Start()
    {
        this.ropeLineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Runs every FixedUpdate. Handles player input with the Grapple.
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && isEnabled)
        {
            if (this.grappling || this.pulling)
            {
                this.retractRopeCoroutine = StartCoroutine(this.retractRope());
            }
            else
            {
                if (this.retractRopeCoroutine != null)
                {
                    StopCoroutine(this.retractRopeCoroutine);
                }
                //Immediately retract rope.
                this.retractingRope = false;
                //Check if can grapple.
                RaycastHit grappleRaycastHitData;
                bool canGrapple = Physics.Raycast(playerRigidbody.position, Camera.main.GetComponent<CameraController>().getMouseLocationInWorldCoordinates() - playerRigidbody.position, out grappleRaycastHitData, 15f, grappleMask);
                if (grappleRaycastHitData.collider == null)
                {
                    this.createMissRope(Camera.main.GetComponent<CameraController>().getMouseLocationInWorldCoordinates());
                }
                //Grapple if possible.
                else if (grappleRaycastHitData.collider.gameObject.layer == LayerMask.NameToLayer("GrappleAllowedTerrain"))
                {
                    this.createGrapplingRope(grappleRaycastHitData);
                }
                //Pull if possible.
                else if (grappleRaycastHitData.collider.gameObject.layer == LayerMask.NameToLayer("PullableObjects"))
                {
                    this.creatingPullingRope(playerRigidbody, grappleRaycastHitData.rigidbody, grappleRaycastHitData.point);
                }
                //If hit objects cant grapple with.
                else
                {
                    this.createMissRope(grappleRaycastHitData.point);
                }
            }
        }
        else if (Input.GetButtonDown("StopUsingUtilityItem"))
        {
            this.retractRopeCoroutine = StartCoroutine(this.retractRope());
        }
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
            this.otherEnd = this.otherObject.position;
            this.drawRope(this.gameObject.transform.position, this.otherEnd);
        }
        else if (this.retractingRope)
        {
            this.drawRope(this.gameObject.transform.position, this.otherEnd);
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
        AudioSource.PlayClipAtPoint(this.grappleShotNoise, this.transform.position, 6.5f);
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
        this.otherObject = secondRigidbody;
        this.otherEnd = secondRigidbody.position;
        this.pulling = true;
        AudioSource.PlayClipAtPoint(this.grappleShotNoise, this.transform.position, 6.5f);
    }

    /// <summary>
    /// This function destroys the rope if the player gets too far from teh object they are pulling.
    /// </summary>
    public void handlePullingRopeStretching()
    {
        if (Vector3.Distance(this.otherEnd, this.playerRigidbody.position) > Grapple.MAX_PULLING_ROPE_LENGTH)
        {
            StartCoroutine(this.retractRope());
        }
    }
    #endregion

    /// <summary>
    /// Creates a miss rope that is immediately retracted.
    /// </summary>
    /// <param name="clickLocation">The location the user clicked on used as the other end of the rope.</param>
    private void createMissRope(Vector3 clickLocation)
    {
        float distance = Vector3.Distance(clickLocation, this.transform.position);
        //If user clicked further than rope length shrink it.
        if (distance > Grapple.MAX_ROPE_LENGTH)
        {
            float xGradient = (clickLocation.x - this.transform.position.x) / distance;
            float yGradient = (clickLocation.y - this.transform.position.y) / distance;

            float xPosition = xGradient * Grapple.MAX_ROPE_LENGTH;
            float yPosition = yGradient * Grapple.MAX_ROPE_LENGTH;

            this.otherEnd = (new Vector3(this.transform.position.x + xPosition, this.transform.position.y + yPosition, 0));
        }
        else
        {
            this.otherEnd = clickLocation;
        }
        this.retractRopeCoroutine = StartCoroutine(this.retractRope());
    }

    /// <summary>
    /// This function destroys all the rope game objects and resets key variables.
    /// </summary>
    public void destroyRopeJoint()
    {
        if (grappling)
        {
            Destroy(this.playerRigidbody.gameObject.GetComponent<ConfigurableJoint>());
            grappling = false;
        }
        if (pulling)
        {
            Destroy(this.otherObjectMover);
        }
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
        this.ropeLineRenderer.SetVertexCount(2);
        this.ropeLineRenderer.SetPositions(new Vector3[] { firstPoint, secondPoint });
        this.ropeLineRenderer.enabled = true;
    }


    /// <summary>
    /// This function retracts the grapple rope and stop the user from grappling once it is disabled.
    /// </summary>
    /// <returns></returns>
    private IEnumerator retractRope()
    {
        AudioSource.PlayClipAtPoint(this.grappleRetractNoise, this.transform.position, 6.5f);
        destroyRopeJoint();
        this.retractingRope = true;
        while (Vector3.Distance(this.otherEnd, this.transform.position) > .3f)
        {
            this.otherEnd = Vector3.MoveTowards(this.otherEnd, this.gameObject.transform.position, .5f);
            yield return new WaitForSeconds(.025f);
        }
        this.retractingRope = false;
        ropeLineRenderer.SetVertexCount(0);
    }

    /// <summary>
    /// Retracts the rope immeditialey when the player wants to fire it while still retracting it from their last fire.
    /// </summary>
    public void retractRopeImmediately()
    {
        ropeLineRenderer.SetVertexCount(0);
        this.retractingRope = false;
        this.pulling = false;
        this.grappling = false;
    }
}