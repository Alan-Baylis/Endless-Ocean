using UnityEngine;
using System.Collections;

public class DialogueOnTrigger : MonoBehaviour {

    private DialogueManager dialogueManager;

    public string[] dialogueLines;
    public string speakerName;
    bool beenTriggered;

    // Use this for initialization
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        beenTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !beenTriggered)
        {
            if (!dialogueManager.dialogueActive)
            {
                beenTriggered = true;
                dialogueManager.showDialogue(this.speakerName, this.dialogueLines,5.0f);

            }
        }
    }
}
