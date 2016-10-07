using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void startGame ()
    {
        //Application.LoadLevel("Sidescrolling Scene");
        SceneManager.LoadScene("Sidescrolling Scene");
    }

    public void loadGame()
    {
        SceneManager.LoadScene("BoatM");
    }

    public void optionMenu ()
    {
        SceneManager.LoadScene("Options");
    }

    public void Exit ()
    {
        Application.Quit();
    }
}
