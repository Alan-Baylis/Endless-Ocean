using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour {

    public void restartGame (string name)
    {
        SceneManager.LoadScene(name);
    }

    public void quit ()
    {
        Application.Quit();

    }
}
