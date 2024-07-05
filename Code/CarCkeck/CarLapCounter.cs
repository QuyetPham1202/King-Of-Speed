using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System;
using UnityEditor.UI;
using TMPro;
using UnityEditor;

public class CarLapCounter : MonoBehaviour
{

    public TextMeshProUGUI carPositionText;
    int passedCheckPointNumber = 0;
    float timeAtLastPassCheckPoint = 0;
    int numberOfPassedCheckpoints = 0;
    bool isRaceCompleted = false;
    int lapsComleted = 0;
    const int lapsToComplete = 2;
    int carPosition = 0;
    public event Action<CarLapCounter> OnPassCheckPoint;
    CheckPoint1 checkPoint;
    public int tinhdiem = 0;
    bool isHideRoutineRunning = false;
    float hideUIDelayTime;
    LapCounterUIHandller lapCounterUIHandller;
    private void Start()
    {
        if (CompareTag("Player"))
        {
            lapCounterUIHandller = FindObjectOfType<LapCounterUIHandller>();
            lapCounterUIHandller.SetLapText($"LAP {lapsComleted + 1}/{lapsToComplete}");
        }
    }
    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }
    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassCheckPoint;
    }
    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();
        carPositionText.gameObject.SetActive(true);
        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;
            yield return new WaitForSeconds(hideUIDelayTime);
            carPositionText.gameObject.SetActive(false);
            isHideRoutineRunning = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted)
                return;
            CheckPoint1 checkPoint = collision.GetComponent<CheckPoint1>();

            if (passedCheckPointNumber + 1 == checkPoint.checPointNumber)
            {
                passedCheckPointNumber = checkPoint.checPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsComleted++;
                    if (lapsComleted >= lapsToComplete)
                        isRaceCompleted = true;
                   tinhdiem = lapsToComplete - lapsComleted;
                 
                  
                    if (!isRaceCompleted && lapCounterUIHandller != null)
                        lapCounterUIHandller.SetLapText($"LAP{lapsComleted + 1}/{lapsToComplete}");
                }
                OnPassCheckPoint?.Invoke(this);
                if (isRaceCompleted)
                {
                    StartCoroutine(ShowPositionCO(100));
                    if (CompareTag("Player"))
                    {
                  
                        GameManager.instance.OnRaceCompler();
                        DisableCarControl();


                    }
                }

                else if (checkPoint.isFinishLine) StartCoroutine(ShowPositionCO(1.5f));
            }
        }
    }
    private void DisableCarControl()
    {
        CarInputHandler carInputHandler = GetComponent<CarInputHandler>();
        if (carInputHandler != null)
        {
            carInputHandler.enabled = false;
        }

        AICar aiCar = GetComponent<AICar>();
        if (aiCar != null)
        {
            aiCar.enabled = false;
        }

        AStarLine aStarLine = GetComponent<AStarLine>();
        if (aStarLine != null)
        {
            aStarLine.enabled = false;
        }
        Debug.Log(" da hoat dong het");

    }
}
