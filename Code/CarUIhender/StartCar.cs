using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCar : MonoBehaviour
{

    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        // Tạo một AudioSource mới
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        // Gán AudioClip cho AudioSource
        audioSource.clip = audioClip;

        // Cài đặt loop thành true để âm thanh lặp lại
        audioSource.loop = true;

        // Phát âm thanh
        audioSource.Play();
    }
}
