using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject button;
    public TextMeshProUGUI buttonText;
    public float hoverScaleFactor = 1.1f;
    public float scaleSpeed = 5f;
    private Vector3 originalScale;
    private Color originalTextColor;
    private Color originalButtonColor;
    private Coroutine scaleCoroutine;

    private void Start()
    {
        originalScale = button.transform.localScale;
        originalTextColor = buttonText.color;
        originalButtonColor = button.GetComponent<Image>().color;
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

    // Các phương thức khác
}
