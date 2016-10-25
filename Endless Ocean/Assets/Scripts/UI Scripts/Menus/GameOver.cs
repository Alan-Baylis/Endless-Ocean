﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {

    public static string previousLevel;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
    }

    public void restartGame ()
    {
        SceneManager.LoadScene(previousLevel);
        transform.position = Vector3.zero;
    }

    public void quit ()
    {
        SceneManager.LoadScene("Menu");

    }
}
