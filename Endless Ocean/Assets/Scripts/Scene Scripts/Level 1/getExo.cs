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
                    dialogueManager.showDialogue(this.speakerName, this.dialogueLines,0.0f);
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
