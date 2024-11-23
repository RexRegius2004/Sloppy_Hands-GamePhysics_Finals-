using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    Scene currentScene;

    
    private void Update()
    {
        CursorUpdate();
    }
    public static void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public static void EndGame()
    {
       
        SceneManager.LoadScene("GameOver");
    }

    public static void MainMenuGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
   
    void CursorUpdate()
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "Game")
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
