using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interactablePrompt : MonoBehaviour {

    public Image prompt;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.CompareTag("PlayerShip"))
        {
            prompt.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            prompt.gameObject.SetActive(false);
        }
    }
}
