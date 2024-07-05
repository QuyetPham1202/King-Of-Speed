using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimeUIHandler : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    float lastRaceTimeUpdate = 0;
    private void Awake() 
    {
        timeText = GetComponent<TextMeshProUGUI>();

    }
    private void Start()
    {
        StartCoroutine(UpdateTimeCO());
    }
    IEnumerator UpdateTimeCO()
    {
        while(true){
            float raceTime =GameManager.instance.GetRaceTime();
            if(lastRaceTimeUpdate != raceTime)
            {
                int raceTimeMinutes = (int)Mathf.Floor(raceTime/ 60);
                int raceTimeSeconds = (int)Mathf.Floor(raceTime% 60);
                timeText.text = $"{raceTimeMinutes.ToString("00")}:{raceTimeSeconds.ToString("00")}";
                lastRaceTimeUpdate = raceTime;

            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
