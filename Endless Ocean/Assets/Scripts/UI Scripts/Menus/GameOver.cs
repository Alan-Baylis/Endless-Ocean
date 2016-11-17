using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour{

    public static string previousLevel;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().maxHealth;
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        PreserveAcrossLevels.playerGuiInstance.transform.GetChild(1).gameObject.SetActive(false);
        if (PreserveAcrossLevels.playerInstance.gameObject.GetComponent<PlayerController>().hasExo)
        {
            PreserveAcrossLevels.playerInstance.gameObject.transform.Find("Exo").GetComponent<SkinnedMeshRenderer>().materials = PreserveAcrossLevels.playerInstance.gameObject.GetComponent<PlayerController>().materialsBackup;
        }
        else
        {
            PreserveAcrossLevels.playerInstance.gameObject.transform.Find("Human").GetComponent<SkinnedMeshRenderer>().materials = PreserveAcrossLevels.playerInstance.gameObject.GetComponent<PlayerController>().materialsBackup;
        }
        PreserveAcrossLevels.playerInstance.gameObject.GetComponent<PlayerController>().flashing = false;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(previousLevel);
        transform.position = Vector3.zero;
    }

    public void quit()
    {
        SceneManager.LoadScene("Menu");

    }
}
