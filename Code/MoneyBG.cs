using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoneyBG : MonoBehaviour
{
    private Vector3 initialPosition;
    private Renderer money;
    private Collider2D Moneycollier;

    private void Start()
    {
        // Lưu vị trí ban đầu của `MoneyBG`
        initialPosition = transform.position;
        money = GetComponent<Renderer>();
        Moneycollier = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        StartCoroutine(HandleMoneyBGRespawn());


    }


    private IEnumerator HandleMoneyBGRespawn()
    {
        // Ẩn `MoneyBG` và vô hiệu hóa collider của nó
        money.enabled = false;
        Moneycollier.enabled = false;

        // Chờ 3 giây
        yield return new WaitForSeconds(3f);

        // Đặt lại vị trí và hiển thị lại `MoneyBG`
        transform.position = initialPosition;
        money.enabled = true;
        Moneycollier.enabled = true;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50);
    }
}