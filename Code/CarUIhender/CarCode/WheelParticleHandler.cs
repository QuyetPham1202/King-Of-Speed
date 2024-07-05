using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;
     TopDownCarControllider topDownCarController;
    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;
    ParticleSystem.MainModule particleSystemMainModule;

    // Start is called before the first frame update
    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarControllider>();
       particleSystemSmoke = GetComponentInParent<ParticleSystem>();
        
            

        particleSystemEmissionModule = particleSystemSmoke.emission;
/*        particleSystemMainModule = particleSystemSmoke.main;*/
        particleSystemEmissionModule.rateOverTime = 0;
    }

    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;
        if(topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                particleEmissionRate = 30;
            else particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}
