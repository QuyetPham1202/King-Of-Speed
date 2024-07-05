using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    public bool isoOerpassEmiter = false;

    TopDownCarControllider topDownCarControllider;
    TrailRenderer trailRenderer;
    CarLayerHandler  carLayerHandler;
    void Awake()
    {
        topDownCarControllider = GetComponentInParent<TopDownCarControllider>();
       
        carLayerHandler = GetComponentInParent<CarLayerHandler>();

        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;

    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        trailRenderer.emitting = false;
        if (topDownCarControllider.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (carLayerHandler.IsDrivingOnOverpass() && isoOerpassEmiter)
                trailRenderer.emitting = true;
            if (!carLayerHandler.IsDrivingOnOverpass() && !isoOerpassEmiter)
                trailRenderer.emitting = true;
        }
    }
}
