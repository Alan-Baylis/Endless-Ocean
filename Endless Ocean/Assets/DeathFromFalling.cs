using UnityEngine;
using System.Collections;

public class DeathFromFalling : MonoBehaviour {

    /// <summary>
    /// Kill player on collision
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameOver.previousLevel = Application.loadedLevelName;
            Application.LoadLevel("Game Over");
        }
    }
}
