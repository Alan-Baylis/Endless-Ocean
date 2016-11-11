using UnityEngine;
using System.Collections;
/// <summary>
/// This class controls the camera in the game. It ensures the camera follows the player.
/// </summary>
public class CameraController : MonoBehaviour {

    private Transform target;
    public float smoothing = 5f;
    public Vector3 offset;
    public bool followPlayer;


	/// <summary>
    /// Runs the first time the camera controller is intialized, before it starts. Initialzes key variables.
    /// </summary>
	void Awake () {
        this.target = PreserveAcrossLevels.playerInstance.gameObject.transform;
        this.followPlayer = true;
	}


    /// <summary>
    /// Runs each fixed update. Makes the camera follow the player.
    /// </summary>    	
	void FixedUpdate () {
        if (followPlayer)
        {
            Vector3 targetCameraPosition = target.position + offset;
            this.transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);
        }
    }

    //HELPER FUNCTIONS
    /// <summary>
    /// This function calculates the mouses location in the games world coordinates system.
    /// </summary>
    /// <returns>The mouses location in world coordinates.</returns>
    public Vector3 getMouseLocationInWorldCoordinates()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 50f;
        Vector3 mouseLocationInWorldCoords = this.gameObject.GetComponent<Camera>().ScreenToWorldPoint(mousePosition);
        mouseLocationInWorldCoords.z = 0;
        return mouseLocationInWorldCoords;
    }
}
