using UnityEngine;
using System.Collections;

public class TutorialMovementScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.R))
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
