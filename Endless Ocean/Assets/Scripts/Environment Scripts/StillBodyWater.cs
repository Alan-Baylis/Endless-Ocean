using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for still bodies of water. It handles the physics simulation for this water.
/// 
/// The formula used for simulating floating comes from this thread https://forum.unity3d.com/threads/floating-a-object-on-water.31671/
/// </summary>
public class StillBodyWater : MonoBehaviour {

    private int floatHeight = 1;
    //Int with the water level where water stops.
    private float waterLevel = 0;
    //An int to decresen the force lifting up objects that 'float'
    private float bounceDamp = 0.5f;


    /// <summary>
    /// Runs whenever an object is inside the water. Adds a buoyancy fornce to the object to make it 'float'.
    /// </summary>
    /// <param name="col">The other collider in the water.</param>
    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Rigidbody>() != null)
        {
            float floatForceFactor = 1f - ((col.gameObject.transform.position.y - this.waterLevel) / floatHeight);
            if(floatForceFactor > 0)
            {
                Vector3 floatForce = -Physics.gravity * (floatForceFactor - col.GetComponent<Rigidbody>().velocity.y * bounceDamp);
                col.GetComponent<Rigidbody>().AddForce(floatForce);
            }
        }
    }

    /// <summary>
    /// Initializes key variables.
    /// </summary>
    void Start()
    {
        this.waterLevel = this.gameObject.transform.position.y;
    }
}
