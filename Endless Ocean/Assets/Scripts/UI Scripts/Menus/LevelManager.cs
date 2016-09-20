using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void startGame ()
    {
        Application.LoadLevel("Sidescrolling Scene");
    }

    public void loadGame()
    {
        Application.LoadLevel("BoatM");
    }

    public void optionMenu ()
    {
        Application.LoadLevel("Options");
    }

    public void Exit ()
    {
        Application.Quit();
    }
}
