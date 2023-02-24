using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
     public float zoomSpeed = 2.0f;
    public float maxZoomLevel = 10.0f;
    public ZoomDirection zoomDirection = ZoomDirection.In;
    public float fadeDuration = 1.0f;

    private float currentZoomLevel = 1.0f;
    private float targetZoomLevel = 1.0f;
    private float fadeTimer = 0.0f;
    private bool isFading = false;

    public enum ZoomDirection
    {
        In,
        Out
    }

    void Start()
    {
        currentZoomLevel = 1.0f;
        targetZoomLevel = 1.0f;
        fadeTimer = 0.0f;
        isFading = false;
    }

    void Update()
    {
        // Determine the zoom direction based on the selected option in the Inspector
    float zoomDir = (zoomDirection == ZoomDirection.In) ? 1.0f : -1.0f;

    // Update the target zoom level based on the zoom direction and zoom speed
    targetZoomLevel = Mathf.Clamp(targetZoomLevel + zoomDir * zoomSpeed * Time.deltaTime, 1.0f, maxZoomLevel);

    // Check if the zoom level has changed
    if (currentZoomLevel != targetZoomLevel)
    {
        // Start the fade-in/out animation
        fadeTimer = 0.0f;
        isFading = true;
    }

    // Update the current zoom level using a lerp function
    if (isFading)
    {
        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);
        currentZoomLevel = Mathf.Lerp(currentZoomLevel, targetZoomLevel, t);
        if (t >= 1.0f)
        {
            // End the fade-in/out animation
            isFading = false;
        }
    }
    else
    {
        // Immediately set the current zoom level to the target zoom level
        currentZoomLevel = targetZoomLevel;
    }

    // Update the camera's field of view based on the current zoom level
    Camera.main.fieldOfView = 60.0f / currentZoomLevel;
    }
}