using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be applied to water. It makes the water rise.
/// </summary>
public class RisingWaterLevel : MonoBehaviour {

    public float amountToRiseBy;
    public float riseIncrement;

    private float currentHeightDifference;
	
	/// <summary>
    /// Raises the water level each frame. 
    /// </summary>
	void Update () {
        if (currentHeightDifference < amountToRiseBy)
        {
            this.currentHeightDifference += riseIncrement;
            Vector3 newPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + riseIncrement, this.gameObject.transform.position.z);
            this.gameObject.transform.position = newPosition;
        }
        else
        {
            this.enabled = false;
        }
	}
}
