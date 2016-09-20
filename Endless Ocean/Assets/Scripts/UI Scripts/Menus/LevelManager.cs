using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public void loadScene (string name)
    {
        Application.LoadLevel("Sidescrolling Scene");
    }

    public void Load ()
    {
        Application.LoadLevel("BoatM");
    }

    public void Exit ()
    {
        Application.Quit();
    }
}
