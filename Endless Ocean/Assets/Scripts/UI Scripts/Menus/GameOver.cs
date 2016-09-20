using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public void restartGame ()
    {
        Application.LoadLevel("Sidescrolling Scene");
    }

    public void quit ()
    {
        Application.LoadLevel("Menu");

    }
}
