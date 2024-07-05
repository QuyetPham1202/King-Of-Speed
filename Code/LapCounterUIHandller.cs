using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LapCounterUIHandller : MonoBehaviour
{
    TextMeshProUGUI LapText;
    // Start is called before the first frame update
    private void Awake()
    {
        LapText = GetComponent<TextMeshProUGUI>();
    }
    public void SetLapText(string text)
    {
        LapText.text = text;
    }
        }
