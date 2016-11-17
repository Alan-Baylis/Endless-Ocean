using UnityEngine;
using System.Collections;

public class DialogueHolder : MonoBehaviour {

    private DialogueManager dialogueManager;

    public string[] dialogueLines; // lines to send to dialogueManager
    public string speakerName; // name to send to dialogueManager

	// Use this for initialization
	void Start () {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Activate chat prompt on clicking E while colliding
    /// </summary>
    /// <param name="col">colliding object</param>
    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!dialogueManager.dialogueActive)
                {
                    dialogueManager.showDialogue(this.speakerName, this.dialogueLines,0.0f);
                }
            }
        }
    }
}
