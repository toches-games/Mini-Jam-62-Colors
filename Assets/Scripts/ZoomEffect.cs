using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ZoomEffect : MonoBehaviour
{
    Volume zoomEffect;

    float velocity;

    private void Awake()
    {
        zoomEffect = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.sharedInstance.tempTime <= 0.5f)
        {
            zoomEffect.weight = Mathf.SmoothDamp(zoomEffect.weight, 1f, ref velocity, 0.25f);
        }

        else
        {
            zoomEffect.weight = Mathf.SmoothDamp(zoomEffect.weight, 0f, ref velocity, 0.25f);
        }
    }
}
