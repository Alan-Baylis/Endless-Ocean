using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves an object towards the player at a gradually increasing speed. This speed is capped at maxSpeed - it is used move objects the player is pulling with the grapple.
/// </summary>
public class GrapplePullObject : MonoBehaviour {

    public float velocity;
    public GameObject objectToMoveTowards;
    public bool moveInYAxis = false;
    private Rigidbody playerRigidbody;

	// Use this for initialization
	void Start () {
	
	}

    /// <summary>
    /// Initalizes key varialbes for the class as mono behaviour classes cannot have constructors.
    /// </summary>
    public void init(GameObject objectToMoveTowards, float velocity, bool moveInYAxis, Rigidbody playerRigidbody)
    {
        this.velocity = velocity;
        this.objectToMoveTowards = objectToMoveTowards;
        this.moveInYAxis = moveInYAxis;
        this.playerRigidbody = playerRigidbody;
    }

	/// <summary>
    /// Runs once each frame and moves the object towards the player.
    /// </summary>
	void Update () {
        if (Vector3.Distance(objectToMoveTowards.transform.position, this.transform.position) > 5) {
            Vector3 direction = (objectToMoveTowards.transform.position - this.transform.position);
            if (!moveInYAxis) {
                direction.y = 0;
            }
            this.GetComponent<Rigidbody>().velocity += Vector3.ClampMagnitude((direction * velocity), .4f);
            Vector3 tempVelocity = new Vector3();
            tempVelocity = this.GetComponent<Rigidbody>().velocity;
            tempVelocity.x = Mathf.Clamp(tempVelocity.x, -12, 12);
            this.GetComponent<Rigidbody>().velocity = tempVelocity;
        }
	}
}
