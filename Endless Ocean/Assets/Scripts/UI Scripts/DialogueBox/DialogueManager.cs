using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text textBody;
    public Text speakerName;

    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentLine;

    public float letterPause = 0.6f;
    public int currentLetter = 0;
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

    IEnumerator TypeText()
    {
        foreach (char letter in dialogueLines[currentLine])
        {
            if (currentLetter != dialogueLines[currentLine].Length)
            {
                textBody.text += dialogueLines[currentLine][currentLetter];
                if (typeSound1 && typeSound2)
                {
                    //SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
                    yield return 0;
                }
                currentLetter++;
                yield return new WaitForSeconds(letterPause);
            }
        }
    }

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
