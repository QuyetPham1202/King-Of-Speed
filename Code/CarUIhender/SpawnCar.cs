using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
        private void Awake()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnCar");
            CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Transform spawnPoint = spawnPoints[i].transform;
                int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");
                foreach (CarData cardata in carDatas)
                {
                    if (cardata.CarUniqueID == playerSelectedCarID)
                    {
                        GameObject playerCar = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);
                        playerCar.GetComponent<CarInputHandler>().playerNumber = i + 1;
                        break;
                    }
                }
            }
        }
}
