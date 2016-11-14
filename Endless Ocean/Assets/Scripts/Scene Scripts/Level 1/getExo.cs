using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for the special interactive object; Exo in scene one
/// </summary>
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



                    GameObject pistolTemp = Instantiate(Resources.Load(Pistol.modelPathLocal)) as GameObject;
                    GameObject clubTemp = Instantiate(Resources.Load(Club.modelPathLocal)) as GameObject;

                    PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().inventory.addItem(pistolTemp.GetComponent<Pistol>());
                    PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().inventory.addItem(clubTemp.GetComponent<Club>());
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
