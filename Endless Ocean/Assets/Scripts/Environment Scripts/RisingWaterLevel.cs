using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be applied to water. It makes the water rise.
/// </summary>
public class RisingWaterLevel : MonoBehaviour {

    public float amountToRiseBy;
    public float riseIncrement;

    public StillBodyWater bodyOfWater;

    private float currentHeightDifference;

    //Where the extra water that is increasing the water level is coming from.
    public GameObject waterSource;


    /// <summary>
    /// Raises the water level each frame. 
    /// </summary>
    void FixedUpdate() {
        if (waterSource.GetComponent<WaterSource>().addingWater) { 
            if (currentHeightDifference < amountToRiseBy)
            {
                this.currentHeightDifference += riseIncrement;
                Vector3 newPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + riseIncrement, this.gameObject.transform.position.z);
                this.gameObject.transform.position = newPosition;
                this.bodyOfWater.waterLevel = this.gameObject.transform.position.y + this.transform.lossyScale.y;
                this.bodyOfWater.floatHeight = this.bodyOfWater.waterLevel;
            }
            else
            {
                this.waterSource.GetComponent<ParticleSystem>().enableEmission = false;
                this.enabled = false;
            }
        }
	}
}
