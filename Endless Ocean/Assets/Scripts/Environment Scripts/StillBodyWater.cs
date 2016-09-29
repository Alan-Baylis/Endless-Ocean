using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class fo srill bodies of water. It handles the physics simulation for this water.
/// </summary>
public class StillBodyWater : MonoBehaviour {

    public int buoyancyForce;

    /// <summary>
    /// Runs whenever an object is inside the water. Adds a buoyancy fornce to the object to make it 'float'.
    /// </summary>
    /// <param name="col">The other collider in the water.</param>
    void OnTriggerStay(Collider col)
    {
        col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, this.buoyancyForce, 0));
        Debug.Log("Contact");
    }

    /// <summary>
    /// Runs whenever an object is inside the water. Adds a buoyancy fornce to the object to make it 'float'.
    /// </summary>
    /// <param name="col">The other collider in the water.</param>
    void OnTriggerEnter(Collider col)
    {
        col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, this.buoyancyForce, 0));
        Debug.Log("Contact");
    }
}
