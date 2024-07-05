using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TurnofMusic : MonoBehaviour
{

    public GameObject turnOnMusic;
    public GameObject turnOffMusic;
    private CarSfxHandler backgroundMusicSource;
    public AudioMixer musicGroup; // Specify the AudioMixerGroup
    bool isMusic;

    void Start()
    {
        // Load CarSfxHandler component
        backgroundMusicSource = FindObjectOfType<CarSfxHandler>();

        // Load music state
       isMusic = true;
    }

    public void TurnOnMusic()
    {
        isMusic = true;
        Debug.Log("Âm nhạc đã bật");
        if (isMusic)
        {
            turnOnMusic.SetActive(true);
            turnOffMusic.SetActive(false);
            musicGroup.SetFloat("Master", 0);
            PlayerPrefs.SetFloat("musicVolume", 0);
            Debug.Log("da chay den day chua");
        }
    }

    public void TurnOffMusic()
    {
        isMusic = false;
        Debug.Log("Âm nhạc đã tắt");
        if (!isMusic)
        {
            turnOffMusic.SetActive(true);
            turnOnMusic.SetActive(false);
            musicGroup.SetFloat("Master",-80);
            PlayerPrefs.SetFloat("musicVolume", -80);
            Debug.Log("da chay den day chua112123123123123");
        }
    }
}