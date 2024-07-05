using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public enum GameStates { countDown, running, receOver }

public class GameManager : MonoBehaviour
{
    private int MoneyCar = 0;
    public static GameManager instance = null;
    // Start is called before the first frame update
    GameStates gameStates = GameStates.countDown;

    float raceStartTime = 0;
    float raceCompletedtime = 0;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Lever1Start()
    {
        gameStates = GameStates.countDown;
        Debug.Log("Lever1 started");
    }

    public GameStates GetGameState()
    {
       
        return gameStates;
    }

    void ChangGameState(GameStates newGameState)
    {
        if (gameStates != newGameState)
        {
            gameStates = newGameState;
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameStates == GameStates.receOver)
        {
            return raceCompletedtime - raceStartTime;
        }
        else
        {
            return Time.time - raceStartTime;
        }
    }

    public void OnRaceStart()
    {
        Debug.Log("OnraceStart");
        raceStartTime = Time.time;
        ChangGameState(GameStates.running);
    }

    public void OnRaceCompler()
    {
        Debug.Log("OnRaceCompleted");
        raceCompletedtime = Time.time;
        ChangGameState(GameStates.receOver);
        PositionHendler positionHandler = FindObjectOfType<PositionHendler>();

        // Lấy ra xe đầu tiên trong danh sách các xe đua
        CarLapCounter firstPlaceCar = positionHandler.carLapCounters.FirstOrDefault();

        // Kiểm tra nếu xe đầu tiên là người chơi và trò chơi chưa kết thúc
        if (firstPlaceCar != null && firstPlaceCar.CompareTag("Player") && gameStates == GameStates.receOver)
        {
            SocerManager.Instance.AddPoints(60);

            // Save the score
            SocerManager.Instance.SaveScore();

            Debug.Log("Player received bonus points!");
        }
        else
        {
            // Keep the starting score
            Debug.Log("Người chơi không về nhất");
        }
   
        SocerManager.Instance.AddPoints(-MoneyCar);
        PlayerPrefs.SetInt("PlayerScore", SocerManager.Instance.GetScore()); // Save the updated score
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSeceneLLoaded;
    }

    // Update is called once per frame
    void OnSeceneLLoaded(Scene scene, LoadSceneMode mode)
    {
        Lever1Start();
    }
}