using UnityEngine;
using System.Collections;

public class DeathFromFalling : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
            if (col.gameObject.name == "Character")
            {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
