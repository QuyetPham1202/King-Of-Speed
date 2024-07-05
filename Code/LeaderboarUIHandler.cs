using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboarUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefed;

    Setleaderboarlteminfo[] setleaderboarlteminfos;
    bool isInitillzed = false;
    Canvas canvas; 
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        GameManager.instance.OnGameStateChanged += OnGameStartChanged;
    }
    private void Start()
    {

        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        setleaderboarlteminfos = new Setleaderboarlteminfo[carLapCounterArray.Length];
        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefed, leaderboardLayoutGroup.transform);

            setleaderboarlteminfos[i] = leaderboardInfoGameObject.GetComponent<Setleaderboarlteminfo>();
            setleaderboarlteminfos[i].SetPositionText($"{i + 1}.");
        }
        Canvas.ForceUpdateCanvases();
        isInitillzed = true;
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        if (!isInitillzed)
            return;
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setleaderboarlteminfos[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }
   void OnGameStartChanged(GameManager gameManager)
    {if(GameManager.instance.GetGameState() == GameStates.receOver)
        {
            canvas.enabled = true;
        }
        
    }
    private void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStartChanged;
    }
}
