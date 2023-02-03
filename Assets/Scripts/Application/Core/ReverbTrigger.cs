using Application;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;

public class ReverbTrigger : MonoBehaviour
{
    public ReverbData data;

    private void OnTriggerEnter(Collider other) {
        FindObjectOfType<ReverbController>().AddData(data);     // adds reverb data from the trigger when entered
    }

    private void OnTriggerExit(Collider other) {
        FindObjectOfType<ReverbController>().RemoveData(data);  // removes reverb data from the trigger when exited
    }
}
