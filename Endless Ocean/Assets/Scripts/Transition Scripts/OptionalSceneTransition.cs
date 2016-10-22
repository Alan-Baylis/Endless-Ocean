using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This script attatches to a collider so that when the player enters it they are able to transition to a new scene.
/// </summary>
public class OptionalSceneTransition : MonoBehaviour
{

    public string sceneName;

    /// <summary>
    /// When the new player is in this collider than can transition to the new scene by pressing 'E'.
    /// </summary>
    /// <param name="col">The collider of the object that entered the trigger collider.</param>
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("PlayerShip"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                SceneManager.LoadScene(this.sceneName);
            }
        }
    }
}
