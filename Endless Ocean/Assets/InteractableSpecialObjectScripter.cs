using UnityEngine;
using System.Collections;

public class InteractableSpecialObjectScripter : MonoBehaviour {

    // dialogue
    private DialogueManager dialogueManager;
    // black fade layer
    FadeEffect blackLayer;
    //prompt
    [SerializeField]
    private interactablePrompt prompt;

    enum ActionOnUse
    {
        destroy,
        hide,
        deactivate,
        nothing
    }

    // what do do after being run
    [SerializeField]
    private ActionOnUse actionOnUse = ActionOnUse.nothing;
    bool activated;
    bool tasksCompleted;

    // hide / unhide objects
    [SerializeField]
    private bool enableObjectVisibiltyToggle;
    [SerializeField]
    private GameObject[] objectsToVisibilityToggle;

    // dialogue before actions
    [SerializeField]
    private bool enableDialogueBefore;
    [SerializeField]
    private string[] dialogueBefore;
    [SerializeField]
    private float dialogueBeforeTime;
    public string dialogueBeforeSpeakerName;
    private bool DBCompleted;

    // dialogue after actions
    [SerializeField]
    private bool enableDialogueAfter;
    [SerializeField]
    private string[] dialogueAfter;
    [SerializeField]
    private float dialogueAfterTime;
    public string dialogueAfterSpeakerName;

    // gift Items
    [SerializeField]
    private bool enableItemGifting;
    [SerializeField]
    private GameObject[] ItemsToGift;

    // fade black
    [SerializeField]
    private bool enableFade;

    //trigger puzzle object
    [SerializeField]
    private bool enablePuzzleObject;
    [SerializeField]
    private PuzzleObject[] puzzleObjectToToggle;



    // Use this for initialization
    void Start () {

        dialogueManager = FindObjectOfType<DialogueManager>();
        blackLayer = FindObjectOfType<FadeEffect>();

        //initial progress tracking bools
        activated = true;
        tasksCompleted = false;
        DBCompleted = !enableDialogueBefore;
    }
	
	// Update is called once per frame
	void Update () {

        //what to do when all scripts have run
        if (tasksCompleted)
        {
            switch (actionOnUse)
            {
                case (ActionOnUse.deactivate):
                    activated = false;
                    prompt.isEnabled = false;
                    break;
                case (ActionOnUse.destroy):
                    Destroy(this);
                    break;
                case (ActionOnUse.hide):
                    this.gameObject.SetActive(false);
                    prompt.isEnabled = false;
                    break;
                case (ActionOnUse.nothing):
                    activated = true;
                    DBCompleted = !enableDialogueBefore;
                    tasksCompleted = false;
                    break;
            }
        }
    }

    /// <summary>
    /// Collider action run from here
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && Input.GetKeyUp(KeyCode.E) && activated && !dialogueManager.dialogueActive)
        {
            if (!DBCompleted && enableDialogueBefore)
            {
                dialogueManager.showDialogue(dialogueBeforeSpeakerName, dialogueBefore, dialogueBeforeTime);
                DBCompleted = true;
            }
            else
            {
                if (enableObjectVisibiltyToggle)
                {
                    toggleGameObjects(objectsToVisibilityToggle);
                }
                if (enableItemGifting)
                {
                    giftItems(ItemsToGift);
                }
                if (enablePuzzleObject)
                {
                    puzzleObjectToToggle.toggle();
                }
                if (enableFade)
                {
                    blackLayer.fadeOutBool = true;
                }
                if (enableDialogueAfter)
                {
                    dialogueManager.showDialogue(dialogueAfterSpeakerName, dialogueAfter, dialogueAfterTime);
                }
                tasksCompleted = true;
            }
        }
    }

    // Add items to the player's inventory
    void giftItems(GameObject[] Items)
    {
        foreach (GameObject item in Items)
        {
            PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().inventory.addItem(item.GetComponent<Item>());
        }
    }

    // Toggle game object visibility
    void toggleGameObjects (GameObject[] objects)
    {
        foreach (GameObject gameObject in objects)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
