using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject player;
    public GameObject buttonPlayer2;  // Changed from Button to GameObject
    public Button button;
    public TextMeshProUGUI buttonText;
    public float hoverScaleFactor = 1.1f;
    public float scaleSpeed = 5f;
    private Vector3 originalScale;
    private Color originalTextColor;
    private Color originalButtonColor;
    private Coroutine scaleCoroutine;
    public GameObject onMenu;
    public GameObject buttonShop;
    public GameObject Shop;
    public GameObject Money;
    public GameObject Carspecifications;
    public GameObject onSetting;
    public GameObject turnOnMusic;
    public GameObject turnOffMusic;
    private void Start()
    {
        originalScale = button.transform.localScale;
        originalTextColor = buttonText.color;
        originalButtonColor = button.GetComponent<Image>().color;
        Shop.SetActive(false);
        Carspecifications.SetActive(false);
        onSetting.SetActive(false);
        turnOnMusic.SetActive(true);
        turnOffMusic.SetActive(false);
    }
    public void TurnOnMusic()
    {
        turnOnMusic.SetActive(true);
        turnOffMusic.SetActive(false);
    }
    public void TurnOffMusic()
    {
        turnOffMusic.SetActive(true);
        turnOnMusic.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleButton(originalScale * hoverScaleFactor));

        // Change text color and button color using hex strings
        Color newTextColor;
        Color newButtonColor;

        // Parse hex color strings
        if (ColorUtility.TryParseHtmlString("#FF4747", out newTextColor))
        {
            buttonText.color = newTextColor;
        }
        else
        {
            Debug.LogError("Failed to parse text color");
        }

        if (ColorUtility.TryParseHtmlString("#3BA226", out newButtonColor))
        {
            button.GetComponent<Image>().color = newButtonColor;
        }
        else
        {
            Debug.LogError("Failed to parse button color");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleButton(originalScale));

        // Revert text color and button color
        buttonText.color = originalTextColor;
        button.GetComponent<Image>().color = originalButtonColor;
    }

    public void OnSetting()
    {
        onSetting.SetActive(true);
    }
    public void ExitSetting()
    {
        onSetting.SetActive(false);
    }
    private IEnumerator ScaleButton(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 currentScale = button.transform.localScale;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * scaleSpeed;
            button.transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime);
            yield return null;
        }

        button.transform.localScale = targetScale;
    }

    public void OnCarspecifications()
    {
        Carspecifications.SetActive(true);
        Shop.SetActive(false);

    }
    public void Player1()
    {
        buttonShop.SetActive(false);
        buttonPlayer2.SetActive(false);
        gameObject.SetActive(false);
        onMenu.SetActive(true);
    }
    public void EXIT()
    {
        buttonShop.SetActive(true);
        buttonPlayer2.SetActive(true);
        gameObject.SetActive(true);
        onMenu.SetActive(false);
    }
    public void ExitCarspecifications()
    {
        Carspecifications.SetActive(false);
        Shop.SetActive(true);
    }
    public void ButtonSHOP()
    {
        player.SetActive(false);
        gameObject.SetActive(false);
        Shop.SetActive(true);
        Money.SetActive(true);
    }

    public void Player2()
    {

        SceneManager.LoadScene("TopDownCarPlayer");
    }
    public void OnExit()
    {
        player.SetActive(true);
        gameObject.SetActive(true);
        Shop.SetActive(false);
        Money.SetActive(false);
    }
}
