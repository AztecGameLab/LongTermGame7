using System;
using UnityEngine;

public class OverwritingAudioTrigger : MonoBehaviour
{
    public ReverbData data;
    public OverwritingAudioController controller;
    public bool isGlobal;

    private void Start()
    {
        if (isGlobal)
        {
            controller.AddData(data);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGlobal)
        {
            return;
        }

        Debug.Log("Collision enter");
        controller.AddData(data);     // adds reverb data from the trigger when entered
    }

    private void OnTriggerExit(Collider other) {
        if (isGlobal)
        {
            return;
        }

        Debug.Log("Collision exit");
        controller.RemoveData(data);  // removes reverb data from the trigger when exited
    }
}
