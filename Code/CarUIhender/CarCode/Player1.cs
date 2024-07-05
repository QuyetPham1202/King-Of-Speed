using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player1 : MonoBehaviour
{
    public TextMeshProUGUI player1;
    public TextMeshProUGUI player2;
    TopDownCarControllider topDownCarController;
    public int playerNumber;
    private void Awake()
    {
        topDownCarController = GetComponent<TopDownCarControllider>();
    }

    private void Start()
    {
        // Example: Show the texts and start the coroutine to hide them after 5 seconds.
        ShowTexts("Player1!", "Player2!");
    }

    public void ShowTexts(string player1Message, string player2Message)
    {
        player1.text = player1Message;
        player1.gameObject.SetActive(true); // Ensure the text is visible

        player2.text = player2Message;
        player2.gameObject.SetActive(true); // Ensure the text is visible

        StartCoroutine(HideTextsAfterDelay(5f)); // Start coroutine to hide texts after 5 seconds
    }

    private IEnumerator HideTextsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        player1.gameObject.SetActive(false); // Hide player text
        player2.gameObject.SetActive(false); // Hide player2 text
    }
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        // Mapping keys: J to left, K to down, I to up, L to right
        if (Input.GetKey(KeyCode.Keypad1))
        {
            inputVector.x = -1; // Left
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            inputVector.x = 1; // Right
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            inputVector.y = 1; // Up
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            inputVector.y = -1; // Down
        }

        topDownCarController.SetInputVector(inputVector);
    }
    private void OnTriggerEnter2D(Collider2D collision1)
    {
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
 /*       inputVector.x = Input.GetAxis("Horizontal_Letters");
        inputVector.y = Input.GetAxis("Vertical_Letters");*/