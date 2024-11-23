using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{ 
    public static GameUI Instance;

    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text tutorial;

    public float gameTime = 180f; 
    private float remainingTime;

    public static int score = 0; 
    public static int highScore = 0; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingTime = gameTime;
        LoadHighScore();
        UpdateScoreText();
        UpdateTimerText();
        tutorial.text = "*Use the SpaceBar to interact and mouse buttons to balance!*";
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        UpdateTimerText();

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
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

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
