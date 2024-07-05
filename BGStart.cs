using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGStart : MonoBehaviour
{
    private CarLapCounter carLapCounter;
    public GameObject FishGame;
    private void Start()
    {
        // Gán giá trị cho biến tham chiếu carLapCounter từ một đối tượng trong scene
        carLapCounter = FindObjectOfType<CarLapCounter>();
        FishGame.SetActive(false);
        if (carLapCounter == null)
        {
            Debug.LogError("Không tìm thấy đối tượng CarLapCounter trong scene!");
        }
    }
    private void Update()
    {
        if (carLapCounter != null && carLapCounter.tinhdiem == 1)
        {
            Debug.Log("Xuat hien tai day ket thuc");
            Invoke("FishStart", 5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gọi hàm DeactivateGameObject() sau 2 giây
        Invoke("DeactivateGameObject", 2f);
    }

    private void FishStart()
    {
        FishGame.SetActive(true);
    }
    // Hàm để vô hiệu hóa gameObject
    private void DeactivateGameObject()
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
