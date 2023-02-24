using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeDuration = 1f; // How long it takes to fade in/out
    public float startAlpha = 1f; // Starting alpha value
    public float endAlpha = 0f; // Target alpha value
    private float currentAlpha; // Current alpha value
    private bool isFadingOut; // Check object is currently fading out
    private Material material; // The material used by the object

    private void Start()
    {
        currentAlpha = startAlpha; // Initialize current alpha value
        material = GetComponent<Renderer>().material; // Get material used by the object
        SetAlpha(currentAlpha); // Set the initial alpha value of the object
        isFadingOut = true; // Start fading out
    }

    private void Update()
    {
        // Calculate the new alpha value
        currentAlpha = Mathf.Lerp(currentAlpha, isFadingOut ? endAlpha : startAlpha, Time.deltaTime / fadeDuration);
        SetAlpha(currentAlpha); // Update the alpha value of the object

        // Check if the object has faded out or faded in completely
        if (currentAlpha >= 1f && isFadingOut)
        {
            isFadingOut = false; // Start fading in
        }
        else if (currentAlpha <= 0f && !isFadingOut)
        {
            isFadingOut = true; // Start fading out
        }
    }

    private void SetAlpha(float alpha)
    {
        // Get the current color of the material
        Color currentColor = material.color;

        // Set the alpha value of the color
        currentColor.a = alpha;

        // Set the color of the material
        material.color = currentColor;
    }
}