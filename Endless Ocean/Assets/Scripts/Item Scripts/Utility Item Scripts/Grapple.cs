using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is the class for the grapple . It is a utility item that has two modes. (Two modes is not typical of utility items.)
/// 
/// 1. It can be used to swing from certain platforms. This mode does override the players movement.
/// 
/// 2. it cam be used to pull objects towards the player. This does not affect the players movement all.
/// </summary>
public class Grapple: MonoBehaviour
{

    #region Grappling Variables
    //VARIABLES USED FOR GRAPPLING
    //A layermask that filters out terrain the player cannot grapple to when check to see if they are able to grapple.
    public LayerMask grappleMask;
    //A layerMask that filters out all object that cant be pulled.
    private int segmentCount;
    private const int JOINTS_PER_UNIT = 1;
    private const float SWING_FORCE = 10f;
    private Vector3[] segmentPositions;
    private GameObject[] joints;
    private static Vector3 Z_ONLY_AXIS = new Vector3(0, 0, 1);
    public bool ropeExists = false;
    private LineRenderer ropeLineRenderer;
    private GameObject otherObject;
    #endregion

    public Rigidbody playerRigidbody;
    public CameraController playerCameraController;

    // Use this for initialization
    void Start()
    {
        this.ropeLineRenderer = this.gameObject.AddComponent<LineRenderer>();
        this.ropeLineRenderer.SetWidth(.02f, .02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("UseUtilityItem") > 0)
        {
            if (!ropeExists)
            {
                this.buildRope(playerCameraController.getMouseLocationInWorldCoordinates());
            }
        }
        else if(Input.GetAxis("StopUsingUtilityItem") > 0)
        {
            this.destroyRope();
        }
    }

    void FixedUpdate()
    {
        //Overriding player movement if rope is active.
        if (ropeExists)
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            playerRigidbody.AddForce(new Vector3(horizontalMove * Grapple.SWING_FORCE, 0, 0));
            float verticalMove = Input.GetAxis("Vertical");
        }
    }

    void LateUpdate()
    {
        this.drawRope();
    }

    //HERLPER FUNCTIONS

    /// <summary>
    /// This functions builds the rope between the player and the opbject they are attatched to.
    /// </summary>
    /// <param name="mouseLocationInWorldCoords"></param>
    /// <param name="playerRigidbody"></param>
    public void buildRope(Vector3 mouseLocationInWorldCoords)
    {
        RaycastHit raycastHitData = new RaycastHit();
        //Firing a raycast to see if the user can grapple on anything.
        bool canGrapple = Physics.Raycast(playerRigidbody.position, mouseLocationInWorldCoords - playerRigidbody.gameObject.transform.position, out raycastHitData, 7f, grappleMask);
        if (canGrapple)
        {
            //Getting number of segments in rope.
            segmentCount = Mathf.FloorToInt(Vector3.Distance(playerRigidbody.position, raycastHitData.point) * Grapple.JOINTS_PER_UNIT);
            //Splitting line renderer to display each segment.
            ropeLineRenderer.SetVertexCount(segmentCount);
            //Initalizing arrays at correct length.
            segmentPositions = new Vector3[segmentCount];
            joints = new GameObject[segmentCount];
            //Setting first and last segment position at the players body and the raycast hit point.
            segmentPositions[0] = playerRigidbody.position;
            segmentPositions[0] = raycastHitData.point;
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
            this.otherObject = raycastHitData.rigidbody.gameObject;
            endJoint.axis = Grapple.Z_ONLY_AXIS;
            ropeExists = true;
        }
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
    public void drawRope()
    {
        if (ropeExists)
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

    /// <summary>
    /// This function destroys all the rope game objects and rests key variables.
    /// </summary>
    public void destroyRope()
    {
        if (ropeExists)
        {
            for (int i = 0; i < joints.Length; i++)
            {
                Destroy(joints[i]);
            }

            segmentPositions = new Vector3[0];
            segmentCount = 0;
            joints = new GameObject[0];
            ropeExists = false;
        }
    } 

}