using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour {

    public void restartGame (string name)
    {
        //SceneManager.LoadScene("Side Scrolling Scene");
        Application.LoadLevel("Sidescrolling Scene");
    }

    public void quit ()
    {
        Application.LoadLevel("Menu");

    }
}
