using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class ZoomEffect : MonoBehaviour
{
    public float zoomInitTime = 0.5f;
    public float zoomSmoothTime = 0.25f;

    Volume zoomEffect;

    float velocity;

    [HideInInspector]
    public float smoothValue;

    private void Awake()
    {
        zoomEffect = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.sharedInstance.tempTime <= zoomInitTime)
        {
            smoothValue = Mathf.SmoothDamp(smoothValue, 1f, ref velocity, zoomSmoothTime);
        }

        else
        {
            smoothValue = Mathf.SmoothDamp(smoothValue, 0f, ref velocity, zoomSmoothTime);
        }
        
        zoomEffect.weight = smoothValue;
    }
}
