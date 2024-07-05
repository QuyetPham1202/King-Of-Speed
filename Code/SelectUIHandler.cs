using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;
public class SelectUIHandler : MonoBehaviour
{
    public GameObject carlever;
    [Header("Car prefab")]
    public GameObject carPrefab;

    [Header("Spawn on")]
    public Transform spawnOnTransform;

    private bool isChangingCar = false;
    private CarData[] carDatas;

    private CarUIHandler carUIHandler = null;
    private int selectedCarIndex = 0;

    public GameObject noSelectUI;
    public TextMeshProUGUI textNoSelectUI;

    void Start()
    {   
        noSelectUI.SetActive(false);
        carDatas = Resources.LoadAll<CarData>("CarData/");
        if (carDatas.Length > 0)
        {
            StartCoroutine(SpawnCarCO(true));
        }
        else
        {
            Debug.LogError("Không tìm thấy CarData trong Resources/CarData/");
        }
    }

    void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPreviousCar();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            OnNextCar();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnSelcetCar();
        }*/
    }

    public void OnPreviousCar()
    {
        if (isChangingCar) return;

        selectedCarIndex--;
        if (selectedCarIndex < 0) selectedCarIndex = carDatas.Length - 1; // Quay về xe cuối cùng nếu chỉ mục < 0

        StartCoroutine(SpawnCarCO(true));
    }

    public void OnNextCar()
    {
        if (isChangingCar) return;

        selectedCarIndex++;
        if (selectedCarIndex >= carDatas.Length) selectedCarIndex = 0; // Quay về xe đầu tiên nếu chỉ mục >= độ dài mảng

        StartCoroutine(SpawnCarCO(false));
    }
    public void OnSelcetCar()
    {
        if(carDatas[selectedCarIndex].index > 0) 
        { 
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.SetInt("P3SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.SetInt("P4SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P5SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.Save();

        carlever.SetActive(true);
        }
        else
        {
            noSelectUI.SetActive(true);
            textNoSelectUI.text = "You can open the car to play";
        }
    }
    public void OnOK()
    {
        noSelectUI.SetActive(false);
    }
    public void Lever1()
    {
        SceneManager.LoadScene("TopDownCarLever2");
    }
    public void Lever2()
    {
        SceneManager.LoadScene("TopDownCarLever1");
    }
    public void Lever3()
    {
        SceneManager.LoadScene("TopDownCarLever3");
    }
    private IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;

        if (carUIHandler != null)
        {
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);
            yield return new WaitForSeconds(0.2f); 
        }


        if (carUIHandler != null)
        {
            Destroy(carUIHandler.gameObject);
        }

        if (selectedCarIndex >= 0 && selectedCarIndex < carDatas.Length)
        {
 
            GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);
            carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();

            if (carUIHandler != null)
            {
                carUIHandler.SetupCar(carDatas[selectedCarIndex]);
                carUIHandler.StartCarEntraceAnimation(isCarAppearingOnRightSide);
            }
            else
            {
                Debug.LogError("Prefab xe không chứa thành phần CarUIHandler.");
            }
        }
        else
        {
            Debug.LogError("Chỉ mục selectedCarIndex nằm ngoài phạm vi của mảng carDatas.");
        }

        yield return new WaitForSeconds(0.4f);
        isChangingCar = false;
    }
}
