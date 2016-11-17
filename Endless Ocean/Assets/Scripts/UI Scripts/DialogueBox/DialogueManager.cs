using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text textBody;
    public Text speakerName;

    public bool dialogueActive;

    // Lines to print
    public string[] dialogueLines;
    public int currentLine;

    public float letterPause = 0.6f;
    public int currentLetter = 0;

    // audio clips to play, currently unused
    public AudioClip typeSound1;
    public AudioClip typeSound2;

    bool onTimer = false;
    float countDown; // 15 seconds
    float timeBetweenMessages;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (onTimer)
        {
            countDown -= Time.deltaTime;
            // countDown has not elapsed, continue progressing
            if (countDown <= 0)
            {
                dialogueBox.SetActive(true);
                currentLine++;
                textBody.text = "";
                currentLetter = 0;
                countDown = timeBetweenMessages; // 15 seconds
            }
        }

            //next dialogue box
            // On R temporarily until I can find a solution
            if (dialogueActive && Input.GetKeyDown(KeyCode.R))
            {
                if (onTimer)
                {
                    dialogueBox.SetActive(false);
                }
                else
                {
                    currentLine++;
                    textBody.text = "";
                    currentLetter = 0;
                }
            }


            // if all lines exhauseted, stop writing message
            if (currentLine >= dialogueLines.Length)
            {
                dialogueBox.SetActive(false);
                dialogueActive = false;
                onTimer = false;
                currentLine = 0;
                textBody.text = "";
                Time.timeScale = 1;
            }
        
        if (dialogueActive)
        {
            StartCoroutine(TypeText());
        }
    }

    /// <summary>
    /// Type string out letter by letter
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeText()
    {
        foreach (char letter in dialogueLines[currentLine])
        {
            if (currentLetter != dialogueLines[currentLine].Length)
            {
                textBody.text += dialogueLines[currentLine][currentLetter];
                // space for sound
                if (typeSound1 && typeSound2)
                {
                    yield return 0;
                }
                currentLetter++;
                yield return new WaitForSeconds(letterPause);
            }
        }
    }

    /// <summary>
    /// activate dialogue
    /// </summary>
    /// <param name="speakerName">Name of speaker</param>
    /// <param name="dialogueLines">string array of lines to print out</param>
    /// <param name="timer">timer, if 0 game pauses, other is time between prompts</param>
    public void showDialogue(string speakerName, string[] dialogueLines, float timer)
    {

        timeBetweenMessages = timer;
        countDown = timeBetweenMessages;

        if (timeBetweenMessages > 0.0f)
        {
            onTimer = true;
        }
        else
        {
            onTimer = false;
            Time.timeScale = 0;
        }

        this.currentLine = 0;
        this.currentLetter = 0;
        this.speakerName.text = speakerName;
        this.dialogueLines = dialogueLines;

        textBody.text = "";
        dialogueActive = true;
        dialogueBox.SetActive(true);
    }
}
