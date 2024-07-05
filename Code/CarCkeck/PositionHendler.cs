using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHendler : MonoBehaviour
{
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    LeaderboarUIHandler leaderboarUIHandler;

    private void Awake()
    {
      
    }
    private void Start()
    {
        leaderboarUIHandler = FindObjectOfType<LeaderboarUIHandler>();
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounter in carLapCounters)
            lapCounter.OnPassCheckPoint += OnPassCheckpoint;
        leaderboarUIHandler = FindObjectOfType<LeaderboarUIHandler>();
        if (leaderboarUIHandler != null)
            leaderboarUIHandler.UpdateList(carLapCounters);

    }
    private void Update()
    {
    
    }
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPosition(carPosition);
        if (leaderboarUIHandler != null)
            leaderboarUIHandler.UpdateList(carLapCounters);


    }
}
