using UnityEngine;
using System.Collections;

public class DeathFromFalling : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameOver.previousLevel = Application.loadedLevelName;
            Application.LoadLevel("Game Over");
        }
    }
}
