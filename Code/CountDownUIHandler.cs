using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CountDownUIHandler : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    // Start is called before the first frame update
    private void Awake()
    {
        countDownText.text = "";

    }
    void Start()
    {
        
        StartCoroutine(CountDownCO());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator CountDownCO()
    {
        yield return new WaitForSeconds(0.3f);
        int counter = 3;
        while (true)
        {
            if(counter != 0)
                countDownText.text = counter.ToString();
            else
            {
                countDownText.text = "GO";
               GameManager.instance.OnRaceStart();
                break;
            }
            counter--;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
