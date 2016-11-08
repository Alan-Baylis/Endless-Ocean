using UnityEngine;
using System.Collections;

public class activateMovementTutorial : MonoBehaviour {

	// Use this for initialization
    // Activate movement tutorial at beginning of game, even on restart
	void Awake () {
        PreserveAcrossLevels.playerGuiInstance.transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Player Related/PlayerHUD Canvas/Tutorial Images/MovementTutorial").SetActive(true);

        if (PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().getExoState())
        {
            GameObject.Find("Level Areas/Exo Room/Game Objects/EXO").SetActive(false);
            GameObject.Find("Level Areas/Exo Room/Game Objects/Scripted Prompts/message1").SetActive(false);
            GameObject.Find("Level Areas/Exo Room/Game Objects/Scripted Prompts/message2").SetActive(false);

        }
    }
}
