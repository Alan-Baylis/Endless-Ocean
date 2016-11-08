using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class getExo : MonoBehaviour {

    private DialogueManager dialogueManager;

    public string[] dialogueLines;
    public string speakerName;
    bool spokenTo = false;
    FadeEffect blackLayer;

    // Use this for initialization
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        blackLayer = FindObjectOfType<FadeEffect>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!dialogueManager.dialogueActive)
                {
                    dialogueManager.showDialogue(this.speakerName, this.dialogueLines,0.0f);
                    spokenTo = true;

                    blackLayer.fadeOutBool = true;
                }

                }

            if (spokenTo == true && blackLayer.fadeOutBool == false)
            {
                PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().setModel(1);
                this.gameObject.SetActive(false);
            }


            

        }

        }
    
}
