using UnityEngine;
using System.Collections;

public class reforgerRescue : MonoBehaviour
{

    private DialogueManager dialogueManager;

    public string[] dialogueLines;
    public string speakerName;
    PlayerController player;

    // Use this for initialization
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!dialogueManager.dialogueActive)
                {
                    dialogueManager.showDialogue(this.speakerName, this.dialogueLines, 0.0f);
                }
                player.reforgerSaved = true;
            }
        }

    }
}