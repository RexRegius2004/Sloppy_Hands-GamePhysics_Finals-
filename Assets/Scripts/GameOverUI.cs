using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameOverUI : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text finalScoreText;

    void Start()
    {
        highScoreText.text = $"High Score: {GameUI.highScore}";
        finalScoreText.text = $"Score: {GameUI.score}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        GameManager.MainMenuGame();
    }
}
