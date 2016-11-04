using UnityEngine;
using System.Collections;

public class activateMovementTutorial : MonoBehaviour {

	// Use this for initialization
    // Activate movement tutorial at beginning of game, even on restart
	void Start () {
        GameObject.Find("Player Related/PlayerHUD Canvas/Tutorial Images/MovementTutorial").SetActive(true);
    }
}
