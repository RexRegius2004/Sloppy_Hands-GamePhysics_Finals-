using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainMenuUI : MonoBehaviour
{
    public Button playButton;
    public Button QuitButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        // Reload the current scene
        GameManager.PlayGame();
    }

    public void QuitGame()
    {
        GameManager.QuitGame();
    }
}
