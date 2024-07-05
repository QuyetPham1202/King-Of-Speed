using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class CarInputHandler : MonoBehaviour
{
    TopDownCarControllider topDownCarController;
    public int playerNumber;
    private Collider2D collider1;
    // Start is called before the first frame update

    private void Awake()
    {
        collider1 = GetComponent<Collider2D>();
        topDownCarController = GetComponent<TopDownCarControllider>();
    }

    void Start()
    {
        StartCoroutine(ResetLayerAfterDelay(5f));
    }

    private IEnumerator ResetLayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
 
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);
        if (Input.GetButtonDown("Jump"))
            topDownCarController.Jump(1f, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision1)
    {
        if (collision1.CompareTag("Echoing"))
        {
            Debug.Log("Da va cham");
            SocerManager.Instance.AddPoints(10);

            // Lưu điểm số vào PlayerPrefs
            int currentScore = SocerManager.Instance.GetScore();
            PlayerPrefs.SetInt("Score", currentScore);
        }
        else if (collision1.CompareTag("ColliderUp"))
        {
            StartCoroutine(TemporarySpeedChange(-5f, 2f));
            if (SocerManager.Instance.GetScore() > 0)
            {
                SocerManager.Instance.AddPoints(-20);
                int currentScore = SocerManager.Instance.GetScore();
                PlayerPrefs.SetInt("Score", currentScore);
            }
            Debug.Log("da say ra va cham tru toc do va tru ca diem ");
        }
        if (collision1.CompareTag("Colliderplayerup"))
        {
            StartCoroutine(TemporarySpeedChange(-5f, 3f));
            Debug.Log("da tru toc do cua player");
        }
    }
    


    private IEnumerator TemporarySpeedChange(float speedChange, float duration)
    {
        float originalMaxSpeed = topDownCarController.maxSpeed;
        topDownCarController.maxSpeed += speedChange;
        yield return new WaitForSeconds(duration);
        topDownCarController.maxSpeed = originalMaxSpeed;
    }
}
