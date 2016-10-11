using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GrappleTutorial : MonoBehaviour {

    public GameObject player;
    public Image tutorialMenu;
    bool active;

    // Use this for initialization
    void Start() {
        active = false;
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
        if (col.tag == "Player" && player.GetComponent<PlayerController>().getExoState())
        {
            this.tutorialMenu.gameObject.SetActive(true);
            active = true;
            Time.timeScale = 0.0f;
        }
    }
}
