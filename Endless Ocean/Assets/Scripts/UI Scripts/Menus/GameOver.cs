using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {

    public void restartGame ()
    {
        SceneManager.LoadScene("Sidescrolling Scene");
        transform.position = Vector3.zero;
    }

    public void quit ()
    {
        SceneManager.LoadScene("Menu");

    }
}
