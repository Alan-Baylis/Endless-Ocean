using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;


	// Use this for initialization
	void Start () {
        this.offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 targetCameraPosition = target.position + offset;

        this.transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);
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
