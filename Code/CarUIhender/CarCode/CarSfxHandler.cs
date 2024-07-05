using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Audio sources")]
    public AudioSource tiresScreeachingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource carHitAudioSource;
    public AudioSource carJumpAudioSoure;
    public AudioSource carJumpLaningAudioSoure;
    TopDownCarControllider topDownCarControllider;
    private bool isMusicEnabled = true;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    void Awake()
    {
        topDownCarControllider = GetComponentInParent<TopDownCarControllider>();
    }

    void Update()
    {
        
            UpdateEngineSFX();
            UpdateTiresScreechingSFX();
        
    }

    void UpdateEngineSFX()
    {
        float velocityMagnitude = topDownCarControllider.GetVelocityMagnitude();

        float desiredEngineVolume = velocityMagnitude * 0.05f;
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.1f, 2f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 3f, 2f);

        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 10);
    }

    void UpdateTiresScreechingSFX()
    {
        if (topDownCarControllider.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                tiresScreeachingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else
        {
            tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (isMusicEnabled)
        {
            float relativeVelocity = collision2D.relativeVelocity.magnitude;
            float volume = relativeVelocity * 0.1f;
            carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
            carHitAudioSource.volume = volume;
            if (!carHitAudioSource.isPlaying)
                carHitAudioSource.Play();
        }
    }


}