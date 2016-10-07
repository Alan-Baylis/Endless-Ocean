using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Terrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
        
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                SceneManager.LoadScene("Dummy Scene");
            }
        }
    }

// Update is called once per frame
void Update () {
	
	}
}
