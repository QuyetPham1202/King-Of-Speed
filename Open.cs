using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RotateRoutine());
    }

    private IEnumerator RotateRoutine()
    {
        while (true)
        {
  
            yield return StartCoroutine(RotateToAngle(60, 4f));
            
            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(RotateToAngle(0, 4f));

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator RotateToAngle(float targetAngle, float duration)
    {
        float startAngle = transform.eulerAngles.z;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float angle = Mathf.Lerp(startAngle, targetAngle, time / duration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
    }
}
