using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interactablePrompt : MonoBehaviour {
    public bool isEnabled;
    public Image prompt;

	// Use this for initialization
	void Start () {
        isEnabled = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerStay(Collider col)
    {
        if (isEnabled)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.CompareTag("PlayerShip"))
            {
                prompt.gameObject.SetActive(true);
            }
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
