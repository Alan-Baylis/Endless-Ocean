using UnityEngine;
using System.Collections;

public class elevatorFirstLevel : MonoBehaviour
{
    bool triggered;
    public Animator elevatorAC;

    public string[] dialogueLines;
    public string speakerName;

    private DialogueManager dialogueManager;

    // Use this for initialization
    void Start()
    {
        triggered = false;
        this.elevatorAC = this.GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !triggered)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                triggered = true;
                this.elevatorAC.SetBool("triggered", true);
                dialogueManager.showDialogue(this.speakerName, this.dialogueLines, 6.0f);
            }
        }
    }
}
