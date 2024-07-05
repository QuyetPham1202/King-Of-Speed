using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Setleaderboarlteminfo : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI direveNameText;
    public void SetPositionText ( string newPosition)
    {
        positionText.text = newPosition;    
    }
    public void SetDriverNameText( string newDriverName)
    {
        direveNameText.text = newDriverName;
    }

}
