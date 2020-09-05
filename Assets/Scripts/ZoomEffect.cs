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
        /* //Por ahora
        if (LevelManager.sharedInstance.currentState == LevelState.Happy)
        {
            LevelManager.sharedInstance.happyTiles.GetComponent<TilemapRenderer>().material.SetFloat("Fade", 1 - smoothValue);
            LevelManager.sharedInstance.sadTiles.GetComponent<TilemapRenderer>().material.SetFloat("Fade", smoothValue);
        }

        else
        {
            LevelManager.sharedInstance.happyTiles.GetComponent<TilemapRenderer>().material.SetFloat("Fade", smoothValue);
            LevelManager.sharedInstance.sadTiles.GetComponent<TilemapRenderer>().material.SetFloat("Fade", 1 - smoothValue);
        }
        //--- */

        if (LevelManager.sharedInstance.tempTime <= zoomInitTime)
        {
            smoothValue = Mathf.SmoothDamp(smoothValue, 1f, ref velocity, zoomSmoothTime);
        }

        else if (LevelManager.sharedInstance.tempTime >= LevelManager.sharedInstance.nextStateTime - zoomInitTime)
        {
            smoothValue = Mathf.SmoothDamp(smoothValue, 0f, ref velocity, zoomSmoothTime);
        }

        else
        {
            //smoothValue = 0;
        }
        
        zoomEffect.weight = smoothValue;
    }
}
