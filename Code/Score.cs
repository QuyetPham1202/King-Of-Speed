using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        // Subscribe to the ScoreChanged event in SocerManager
        SocerManager.Instance.ScoreChanged += UpdateScoreText;
        // Update score text initially
    }
    private void Update()
    { 
        UpdateScoreText(SocerManager.Instance.GetScore());

    }

    // Method to update the score text
    private void UpdateScoreText(int score)
    {

        scoreText.text = ""+score;
    }
   
}
