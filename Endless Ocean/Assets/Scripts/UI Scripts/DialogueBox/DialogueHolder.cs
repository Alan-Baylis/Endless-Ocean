using UnityEngine;
using System.Collections;

public class DialogueHolder : MonoBehaviour {

    private DialogueManager dialogueManager;

    public string[] dialogueLines;

	// Use this for initialization
	void Start () {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!dialogueManager.dialogueActive)
                {
                    Time.timeScale = 0;
                    dialogueManager.currentLine = 0;
                    dialogueManager.currentLetter = 0;
                    dialogueManager.dialogueLines = this.dialogueLines;
                    dialogueManager.showDialogue();
                }
            }
        }
    }
}
