using UnityEngine;
using System.Collections;
/// <summary>
/// This class resets the player's position when a new scene loads.
/// </summary>
public class PlayerInitializer : MonoBehaviour {

    public Vector3 positionToStartAt;

    /// <summary>
    /// Resets the players position and enables the player game objects.
    /// </summary>
	void Start () {
        PreserveAcrossLevels[] preservedObjects = Resources.FindObjectsOfTypeAll<PreserveAcrossLevels>();

        foreach(PreserveAcrossLevels preservedObject in preservedObjects)
        {
            preservedObject.gameObject.SetActive(true);
        }

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
