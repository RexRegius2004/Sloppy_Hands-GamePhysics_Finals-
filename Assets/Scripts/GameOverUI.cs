using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text highScoreText; // TextMeshPro for high score display on game over screen
    public TMP_Text finalScoreText; // TextMeshPro for the final score display
    public Button retryButton; // Retry button to restart the game
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = $"High Score: {GameUI.highScore}";
        finalScoreText.text = $"Score: {GameUI.score}";
        retryButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RestartGame()
    {
        // Reload the current scene
        GameManager.MainMenuGame();
    }
}
