using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    public static GameUI Instance; // Singleton instance

    public TMP_Text scoreText; // TextMeshPro for score
    public TMP_Text timerText; // TextMeshPro for timer
    public TMP_Text tutorial;


    public float gameTime = 180f; // Total game time (in seconds)

    public static int score = 0; // Player score
    private float remainingTime; // Remaining time in the game
    public bool isGameActive = true; // Flag to check if the game is active
    public static int highScore = 0; // Stores the high score

    public bool IsGameActive => isGameActive; // Property to expose the game state

    private void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {


        // Retrieve the name of this scene.

        remainingTime = gameTime;
        LoadHighScore();
        UpdateScoreText();
        UpdateTimerText();
        tutorial.text = "*Use the SpaceBar to interact and mouse buttons to balance!*";
 // Add listener to retry button
    }

    private void Update()
    {
        if (!isGameActive) return;

        // Update the timer
        remainingTime -= Time.deltaTime;
        UpdateTimerText();

        // End the game if the timer runs out
        if (remainingTime <= 0)
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(tutorial);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }



    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.Max(0, Mathf.FloorToInt(remainingTime))}s";
        }
    }

    private void EndGame()
    {
        isGameActive = false;

        // Update score screen
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

        // Change scene to "GameOver"
        LoadHighScore();
        GameManager.EndGame();
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
