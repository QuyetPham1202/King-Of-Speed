using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using System;

public class Carshop : MonoBehaviour
{
    SocerManager socerManager;
    public int carUniqueID; 
    public Sprite carUISprite; 
    public GameObject carPrefab; 
    public CarData carData;
    private int  MoneyCar;
    public TextMeshProUGUI money;
    public GameObject buyCurvedBars;
    public TextMeshProUGUI textBuyCurvedBars;
    public TextMeshProUGUI textButMesshPro;
    public GameObject noBuyCar;
    private bool own;

    public void Awake()
    {
        UpdateMoneyText();
        buyCurvedBars.SetActive(false);
        textButMesshPro.text = "Buy";
        own = CheckIfCarOwned();
        if (own)
        {
            textButMesshPro.text = "OK";
            noBuyCar.SetActive(true);
        }
        else
        {
            textButMesshPro.text = "Buy";
            noBuyCar.SetActive(false);
        }

    }
    private void Start()
    {
        Debug.Log("Giá trị của carUniqueID: " + carData.carUniqueID);

    }
    private bool CheckIfCarOwned()
    {

        if (carData.carUniqueID == carUniqueID)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
    public void OnButton()
    {
        if (SocerManager.Instance.GetScore() >= MoneyCar)
        {
   
            if (!own)
            {
      
                carData.index = 1;
                carData.carUniqueID = carUniqueID;
                carData.carUISprite = carUISprite;
                carData.carPrefab = carPrefab;

                
                SaveCarData(carData);

                SocerManager.Instance.AddPoints(-MoneyCar);
                SocerManager.Instance.SaveScore();


                buyCurvedBars.SetActive(true);
                textBuyCurvedBars.text = "Purchase successful!";
                textButMesshPro.text = "OK";
                noBuyCar.SetActive(true);
                own = true;

    
                UpdateMoneyText();


                SaveUIState();
            }
            else
            {
              
                buyCurvedBars.SetActive(true);
                textBuyCurvedBars.text = "You already own this car.";
                textButMesshPro.text = "OK";
                noBuyCar.SetActive(true);
            }
        }
        else
        {

            buyCurvedBars.SetActive(true);
            textBuyCurvedBars.text = "Not enough points to purchase the car.";
        }
    }
    
    public void OK()
    {
        buyCurvedBars.SetActive(false);
    }
    
    private void SaveCarData(CarData carData)
    {
      
        CarDataManager.Instance.AddCarData(carData);
    }
    public void UpdateMoneyText()
    {
        MoneyCar = carUniqueID * 150;
    
        money.text = MoneyCar.ToString();
    }
    private void SaveUIState()
    {
        PlayerPrefs.SetInt("Car_" + carUniqueID + "_noBuyCar", noBuyCar.activeSelf ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void RestoreUIState()
    {
        noBuyCar.SetActive(PlayerPrefs.GetInt("Car_" + carUniqueID + "_noBuyCar", 0) == 1);
    }
}