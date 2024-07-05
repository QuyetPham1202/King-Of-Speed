using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SocerManager : MonoBehaviour
{
    private int score;
    private int scorestart;


    public static SocerManager Instance { get; private set; }


    public delegate void ScoreChangedDelegate(int newScore);
    public event ScoreChangedDelegate ScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }


        score = PlayerPrefs.GetInt("Score", 0);
    }
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddPoints(500); 
            SaveScore();    
        }
    }
    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("Score: " + score);

        ScoreChanged?.Invoke(score);
    }


    public int GetScoreStart()
    {
        return scorestart;
    }


    public int GetScore()
    {
        return score;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save(); 
    }
}