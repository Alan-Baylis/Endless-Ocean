using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class for still bodies of water. It handles the physics simulation for this water.
/// 
/// The formula used for simulating floating comes from this thread https://forum.unity3d.com/threads/floating-a-object-on-water.31671/
/// </summary>
public class StillBodyWater : MonoBehaviour
{

    public float floatHeight;
    //Int with the water level where water stops.
    public float waterLevel;
    //An int to decresen the force lifting up objects that 'float'
    private float bounceDamp;

    private float moveSpeedRemoved;




    /// <summary>
    /// Runs whenever an object is inside the water. Adds a buoyancy fornce to the object to make it 'float'.
    /// </summary>
    /// <param name="col">The other collider in the water.</param>
    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Rigidbody>() != null && !col.gameObject.CompareTag("Player"))
        {
            float floatForceFactor = 1f - ((col.gameObject.transform.position.y - this.waterLevel) / floatHeight);
            if (floatForceFactor > 0)
            {
                Vector3 floatForce = -Physics.gravity * (floatForceFactor - col.GetComponent<Rigidbody>().velocity.y * bounceDamp);
                //Making the float force bigger if it is very small to make the objects more floaty.
                //if (floatForce.y > 9 && floatForce.y < 10)
                //{
                //    floatForce.y--;
                //}
                col.GetComponent<Rigidbody>().AddForce(floatForce);
            }
        }
    }

    ///// <summary>
    ///// Makes the player move slower and jump higher in water.
    ///// </summary>
    ///// <param name="other">The other collider.</param>
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        this.moveSpeedRemoved = (other.GetComponent<PlayerController>().movementSpeed / 2);
    //        other.gameObject.GetComponent<PlayerController>().movementSpeed /= 2;
    //        other.gameObject.GetComponent<Rigidbody>().mass /= 2;
    //    }
    //}

    ///// <summary>
    ///// Returns the players movement to normal when they leave the water.
    ///// </summary>
    ///// <param name="other">The other collider.</param>
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.gameObject.GetComponent<PlayerController>().movementSpeed += this.moveSpeedRemoved;
    //        other.gameObject.GetComponent<Rigidbody>().mass *= 2;
    //    }
    //}

    /// <summary>
    /// Initializes key variables.
    /// </summary>
    void Start()
    {
        this.waterLevel = this.gameObject.transform.position.y + this.transform.lossyScale.y;
        this.floatHeight = this.waterLevel;
        this.bounceDamp = 1f;
    }
}
