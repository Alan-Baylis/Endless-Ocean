using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class getExo : MonoBehaviour {

    private DialogueManager dialogueManager;

    public string[] dialogueLines;
    public string speakerName;
    bool spokenTo = false;
    public GameObject human;
    public GameObject exo;
    public Image blackLayer;

    // Use this for initialization
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
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
                    Time.timeScale = 0;
                    dialogueManager.currentLine = 0;
                    dialogueManager.currentLetter = 0;
                    dialogueManager.speakerName.text = this.speakerName;
                    dialogueManager.dialogueLines = this.dialogueLines;
                    dialogueManager.showDialogue();
                    spokenTo = true;

                    blackLayer.GetComponent<FadeEffect>().fadeOutBool = true;
                }

                }

            if (spokenTo == true && blackLayer.GetComponent<FadeEffect>().fadeOutBool == false)
            {
                human.SetActive(false);
                exo.SetActive(true);
                this.gameObject.SetActive(false);
            }


            

        }

        }
    
}
