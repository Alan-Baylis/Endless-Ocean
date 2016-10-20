using UnityEngine;
using System.Collections;

/// <summary>
/// This script runs when the scene starts. It disables the player game objects. 
/// </summary>
public class OverworldInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        GameObject.FindGameObjectWithTag("Player Related").SetActive(false);
    }

}
