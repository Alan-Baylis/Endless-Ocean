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
    //Vars for creating the grappling rope.
    private int segmentCount;
    private const int JOINTS_PER_UNIT = 2;
    private const float SWING_FORCE = 10f;
    private Vector3[] segmentPositions;
    private GameObject[] joints;
    private static Vector3 Z_ONLY_AXIS = new Vector3(0, 0, 1);
    //Bools indicating what the grapple is doing.
    public bool grappling = false;
    public bool pulling = false;
    //The line rendere that draws the grapples rope.
    private static LineRenderer ropeLineRenderer;
    //A reference to the other object the grapple is attatched to.
    private Rigidbody otherObject;
    //Vars and const for creating the pulling rope.
    private float ropeLength = 4f;
    private const float LINEAR_LIMIT_SPRING = 100f;
    private const float LINEAR_LIMIT_DAMPER = 100f;

    private const float OBJECT_PULL_SPEED = .1f;
    MoveTowardsObject otherObjectMover;

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
                bool canGrapple = Physics.Raycast(playerRigidbody.position, playerCameraController.getMouseLocationInWorldCoordinates() - playerRigidbody.position, out raycastHitData, 7f, grappleMask);
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
            playerRigidbody.AddForce(new Vector3(horizontalMove * Grapple.SWING_FORCE, 0, 0));
        }
    }

    void LateUpdate()
    {
        if (grappling)
        {
            this.drawGrapplingRope();
        }
        else if (pulling)
        {
            this.drawPullingRope(playerRigidbody.position, otherObject.position);
        }
    }

    //HERLPER FUNCTIONS
    #region Grappling Rope Functions
    /// <summary>
    /// This functions builds the rope between the player and the opbject they are attatched to.
    /// </summary>
    /// <param name="mouseLocationInWorldCoords"></param>
    public void createGrapplingRope(Vector3 mouseLocationInWorldCoords, RaycastHit raycastHitData)
    {
        Debug.Log("MakingRope");
        //Firing a raycast to see if the user can grapple on anything.
        //Getting number of segments in rope.
        segmentCount = Mathf.FloorToInt(Vector3.Distance(playerRigidbody.position, raycastHitData.point) * Grapple.JOINTS_PER_UNIT);
        //Splitting line renderer to display each segment.
        ropeLineRenderer.SetVertexCount(segmentCount);
        //Initalizing arrays at correct length.
        segmentPositions = new Vector3[segmentCount];
        joints = new GameObject[segmentCount];
        //Setting first and last segment position at the players body and the raycast hit point.
        segmentPositions[0] = playerRigidbody.position;
        segmentPositions[segmentCount - 1] = raycastHitData.point;
        //Claculating the distnace separating the segments.
        Vector3 segmentSeparation = ((raycastHitData.point - playerRigidbody.position) / (segmentCount - 1));
        //Positiong segments.
        for (int i = 1; i < segmentCount; i++)
        {
            Vector3 position = (segmentSeparation * i) + playerRigidbody.position;
            position.z = 0;
            segmentPositions[i] = position;
            ropeLineRenderer.SetPosition(i, position);
            this.addPhysicsToJoint(i);
        }

        //Attaching joint to body hit by raycast.
        HingeJoint endJoint = raycastHitData.rigidbody.gameObject.AddComponent<HingeJoint>();
        endJoint.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody>();
        this.otherObject = raycastHitData.rigidbody;
        endJoint.axis = Grapple.Z_ONLY_AXIS;
        grappling = true;
    }

    /// <summary>
    /// Creates the joint game objects and configures the mto be 'rope-like'
    /// </summary>
    /// <param name="index">The index of the joint to configure.</param>
    private void addPhysicsToJoint(int index)
    {
        joints[index] = new GameObject("RopeJoint_" + index);
        joints[index].layer = LayerMask.NameToLayer("DoesNotCollideWithPlayer");
        joints[index].transform.position = segmentPositions[index];
        Rigidbody rigidbody = joints[index].AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        SphereCollider collider = joints[index].AddComponent<SphereCollider>();
        collider.radius = .2f;
        HingeJoint joint = joints[index].AddComponent<HingeJoint>();
        joint.axis = Grapple.Z_ONLY_AXIS;
        if(index == 1)
        {
            joint.connectedBody = playerRigidbody;
        }
        else
        {
            joint.connectedBody = joints[index - 1].GetComponent<Rigidbody>();
        }
    }
    
    /// <summary>
    /// This function draws all the joints in the rope using a line renderer.
    /// </summary>
    public void drawGrapplingRope()
    {
        if (grappling)
        {
            for(int i = 0; i < segmentCount; i++)
            {
                if (i == 0)
                {
                    ropeLineRenderer.SetPosition(i, this.transform.position);
                }
                else if (i == segmentCount)
                {
                    ropeLineRenderer.SetPosition(i, this.otherObject.transform.position);
                }
                else
                {
                    ropeLineRenderer.SetPosition(i, joints[i].transform.position);
                }
            }
            ropeLineRenderer.enabled = true;
        }
        else
        {
            ropeLineRenderer.enabled = false;
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
        this.otherObjectMover = secondRigidbody.gameObject.AddComponent<MoveTowardsObject>();
        otherObjectMover.init(firstRigidbody.gameObject, Grapple.OBJECT_PULL_SPEED, this);
        //ConfigurableJoint grappleJoint = firstRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        ////Setting movement restrictions on joint.
        //grappleJoint.autoConfigureConnectedAnchor = false;
        //grappleJoint.xMotion = ConfigurableJointMotion.Limited;
        //grappleJoint.yMotion = ConfigurableJointMotion.Limited;
        ////Adding second body.
        //grappleJoint.connectedBody = secondRigidbody;
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
    /// Adds a line renderer that looks like a rope between two points.
    /// </summary>
    /// <param name="firstPoint">The first point for the rope.</param>
    /// <param name="secondPoint">The second point for the rope.</param>
    public void drawPullingRope(Vector3 firstPoint, Vector3 secondPoint)
    {
        Grapple.ropeLineRenderer.SetVertexCount(2);
        Grapple.ropeLineRenderer.SetPositions(new Vector3[] { firstPoint, secondPoint });
        Grapple.ropeLineRenderer.enabled = true;
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
            for (int i = 0; i < joints.Length; i++)
            {
                Destroy(joints[i]);
            }
            segmentPositions = new Vector3[0];
            segmentCount = 0;
            joints = new GameObject[0];
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
}