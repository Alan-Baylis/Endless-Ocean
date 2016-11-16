using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This class allows the player to transition to the overworld from the ship.
/// </summary>
public class TransitionToOverworldFromShip : MonoBehaviour {

    void FixedUpdate()
    {
        if (Input.GetButtonDown("BoatTransition"))
        {
            SceneManager.LoadScene("BoatM");
        }
    }
}
