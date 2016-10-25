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

        PlayerController playerController = PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>();
        playerController.health = playerController.stamina * 10;

        if (positionToStartAt == null)
        {
            PreserveAcrossLevels.playerInstance.transform.position = Vector3.zero;
        }
        else
        {
            PreserveAcrossLevels.playerInstance.transform.position = positionToStartAt;
        }
	}
}
