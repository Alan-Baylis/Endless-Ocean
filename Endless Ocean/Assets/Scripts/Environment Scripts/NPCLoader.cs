using UnityEngine;
using System.Collections;

/// <summary>
/// Adds the NPCS the player has rescued to the ship.
/// </summary>
public class NPCLoader : MonoBehaviour {

    public GameObject reforger;

	/// <summary>
    /// Runs when the scene loads. Loads the NPCS.
    /// </summary>
	void Start () {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player.reforgerSaved)
        {
            reforger.SetActive(true);
        }
        else
        {
            reforger.SetActive(false);
        }
	}
}
