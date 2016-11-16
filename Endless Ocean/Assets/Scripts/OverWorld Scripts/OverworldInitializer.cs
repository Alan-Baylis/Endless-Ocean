using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This script runs when the scene starts. It disables the player game objects. 
/// </summary>
public class OverworldInitializer : MonoBehaviour {

    public static Vector3 overworldBoatPosition;

	/// <summary>
    /// Sets up the player objects in the scene.
    /// </summary>
	void Start () {
        if (overworldBoatPosition != null && overworldBoatPosition != Vector3.zero)
        {
            GameObject.FindGameObjectWithTag("PlayerShip").transform.position = overworldBoatPosition;
        }
        PreserveAcrossLevels.playerInstance.gameObject.SetActive(false);
        PreserveAcrossLevels.playerGuiInstance.gameObject.SetActive(false);
    }

    /// <summary>
    /// Runs each FixedUpdate. Moves the player to the boat if they press the B button.
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetButtonDown("BoatTransition"))
        {
            overworldBoatPosition = GameObject.FindGameObjectWithTag("PlayerShip").transform.position;
            SceneManager.LoadScene("Ship Scene");
        }
    }

}
