using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string gameScene;
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameBrain.main.StartGame(gameScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Settings()
    {
        GameBrain.main.OpenSettings();
    }
}
