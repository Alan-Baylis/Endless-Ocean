using UnityEngine;
using System.Collections;
/// <summary>
/// This class resets the player's position when a new scene loads.
/// </summary>
public class PlayerInitializer : MonoBehaviour {

    public Vector3 positionToStartAt;

    /// <summary>
    /// Resets the players position.
    /// </summary>
	void Start () {
        if (positionToStartAt == null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = positionToStartAt;
        }
	}
}
