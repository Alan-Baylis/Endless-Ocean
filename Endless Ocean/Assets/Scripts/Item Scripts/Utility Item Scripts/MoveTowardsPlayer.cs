using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves an object towards the player - it is used to move pulled objects towards the player.
/// </summary>
public class MoveTowardsObject : MonoBehaviour {

    public float velocity;
    public GameObject objectToMoveTowards;
    private Grapple callingGrapple;

	// Use this for initialization
	void Start () {
	
	}

    /// <summary>
    /// Initalizes key varialbes for the class as mono behaviour classes cannot have constructors.
    /// </summary>
    public void init(GameObject objectToMoveTowards, float velocity, Grapple callingGrapple)
    {
        this.velocity = velocity;
        this.objectToMoveTowards = objectToMoveTowards;
        this.callingGrapple = callingGrapple;
    }

	/// <summary>
    /// Runs once each frame and moves the object towards the player.
    /// </summary>
	void Update () {
        if (Vector3.Distance(objectToMoveTowards.transform.position, this.transform.position) > 2) {
            Vector3 direction = (objectToMoveTowards.transform.position - this.transform.position);
            direction.y = 0;
            this.GetComponent<Rigidbody>().velocity += Vector3.ClampMagnitude((direction * velocity), .4f);
            Vector3 tempVelocity = new Vector3();
            tempVelocity = this.GetComponent<Rigidbody>().velocity;
            tempVelocity.x = Mathf.Clamp(tempVelocity.x, -3f, 3f);
            this.GetComponent<Rigidbody>().velocity = tempVelocity;
            Debug.Log(tempVelocity);
            Debug.Log(this.GetComponent<Rigidbody>().velocity);
        }
	}
}
