using Application;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;

public class ReverbController : MonoBehaviour
{
    public LinkedList<ReverbData> _reverbData = new LinkedList<ReverbData>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _reverbData.Clear();
        _reverbData.AddFirst(new ReverbData(0f));    // starts with reverb level of 0
    }

    public void AddData(ReverbData data)
    {
        _reverbData.AddFirst(data);                                     // adds new reverbData to front of list
        var errorCode = RuntimeManager.GetBus("bus:/Reverb").setVolume(data.Level);     // sets the reverb to the new data level
        if (errorCode == RESULT.OK)
            UnityEngine.Debug.Log("OK");
    }

    public void RemoveData(ReverbData data)
    {
        _reverbData.Remove(data);                                                           // removes the specified reverb data from the list
        RuntimeManager.GetBus("bus:/Reverb").setVolume(_reverbData.First.Value.Level);      // sets the new reverb level to the current first element of list
    }
}

[System.Serializable]
public class ReverbData
{
    public float Level;

    public ReverbData(float level)
    {
        Level = level;
    }
}
