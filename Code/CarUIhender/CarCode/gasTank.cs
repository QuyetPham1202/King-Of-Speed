using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class gasTank : MonoBehaviour
{
    private Vector3 initialPosition;
    private Renderer gasTankRenderer;
    private Collider2D gasTankCollider;

    private void Start()
    {
        // Store the initial position of the gas tank
        initialPosition = transform.position;
        gasTankRenderer = GetComponent<Renderer>();
        gasTankCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
  
            // Start the coroutine to handle the disappearance and reappearance upon any collision
            StartCoroutine(HandleGasTankRespawn());
        
    }
  
    private IEnumerator HandleGasTankRespawn()
    {
        // Make the gas tank invisible and disable its collider
        gasTankRenderer.enabled = false;
        gasTankCollider.enabled = false;

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Reset the position and make the gas tank visible again
        transform.position = initialPosition;
        gasTankRenderer.enabled = true;
        gasTankCollider.enabled = true;
    }
}
