using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CarDataManager : MonoBehaviour
{
    private static CarDataManager instance;
    public static CarDataManager Instance
    {
        get
        {
            if (instance == null)
            {
          
                GameObject singletonObject = new GameObject("CarDataManager");
                instance = singletonObject.AddComponent<CarDataManager>();
                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    private List<CarData> carDataList = new List<CarData>();


    public void AddCarData(CarData carData)
    {
        if (!IsCarOwned(carData.carUniqueID))
        {
            carDataList.Add(carData);
            SaveCarOwnership(carData.carUniqueID);
        }
    }


    public List<CarData> GetCarDataList()
    {
        return carDataList;
    }


    public bool IsCarOwned(int carUniqueID)
    {
     
        foreach (CarData carData in carDataList)
        {
         
            if (carData.carUniqueID == carUniqueID)
            {
           
                return true;
            }
        }
    
        return false;
    }

    private void SaveCarOwnership(int carUniqueID)
    {
        PlayerPrefs.SetInt("CarOwned_" + carUniqueID, 1);
        PlayerPrefs.Save();
    }
}
