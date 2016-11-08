using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GrappleTutorial : MonoBehaviour {
    GameObject tutorialMenu;
    bool active;
    string path = "Player Related/PlayerHUD Canvas/Tutorial Images/";

    public enum tutorialType
    {
        Grapple,
        Inventory,
        InventoryQS,
        Combat,
        Puzzle,
        Lvlup,
    }

    public tutorialType tuType;

    // Use this for initialization
    void Start() {
        active = false;

        switch(tuType)
        {
            case tutorialType.Grapple:
                this.tutorialMenu = GameObject.Find(path+"GrappleTutorial");
                break;
            case tutorialType.Inventory:
                this.tutorialMenu = GameObject.Find(path + "InventoryTutorial");
                break;
            case tutorialType.InventoryQS:
                this.tutorialMenu = GameObject.Find(path + "InventoryQSTutorial");
                break;
            case tutorialType.Combat:
                this.tutorialMenu = GameObject.Find(path + "CombatTutorial");
                break;
            case tutorialType.Puzzle:
                this.tutorialMenu = GameObject.Find(path + "GrapplePuzzleTutorial");
                break;
            case tutorialType.Lvlup:
                this.tutorialMenu = GameObject.Find(path + "lvlUpTutorial");
                break;

        }
    }

    // Update is called once per frame
    void Update() {
        if(active)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                this.tutorialMenu.gameObject.SetActive(false);
                active = false;
                Time.timeScale = 1.0f;
                this.gameObject.SetActive(false);
            }
        }

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && PreserveAcrossLevels.playerInstance.GetComponent<PlayerController>().getExoState())
        {
            this.tutorialMenu.gameObject.SetActive(true);
            active = true;
            Time.timeScale = 0.0f;
        }
    }
}
