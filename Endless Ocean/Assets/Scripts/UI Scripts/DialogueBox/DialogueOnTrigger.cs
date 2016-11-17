using UnityEngine;
using System.Collections;

public class DialogueOnTrigger : MonoBehaviour {

    private DialogueManager dialogueManager;

    [SerializeField]
    private string[] dialogueLines; // lines to send to dialogueManager
    [SerializeField]
    private string speakerName; // name to send to dialogueManager
    [SerializeField]
    private float timeToDisplay; // time that each prompt will be displayed for

    private bool beenTriggered;

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

    /// <summary>
    /// Check for collision with player, if so start dialogue
    /// </summary>
    /// <param name="col">colliding object</param>
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !beenTriggered)
        {
            if (!dialogueManager.dialogueActive)
            {
                beenTriggered = true;
                dialogueManager.showDialogue(this.speakerName, this.dialogueLines, this.timeToDisplay);

            }
        }
    }
}
