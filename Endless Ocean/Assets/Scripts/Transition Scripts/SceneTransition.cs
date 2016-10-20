using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This script attatches to a collider so that when the player enters it they transition to a new scene.
/// </summary>
public class SceneTransition : MonoBehaviour {

    public string sceneName;

    /// <summary>
    /// Loads the new scene when the player enters the trigger collider.
    /// </summary>
    /// <param name="col">The collider of the object that entered the trigger collider.</param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(this.sceneName);
        }
    }
}
