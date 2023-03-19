using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class OverwritingAudioController : MonoBehaviour
{
    public LinkedList<ReverbData> _reverbData = new LinkedList<ReverbData>();

    public void AddData(ReverbData data)
    {
        if (_reverbData.Count > 0)
        {
            _reverbData.First.Value.instance.setParameterByName("Intensity", 0);
        }

        data.instance = RuntimeManager.CreateInstance(data.reverbSnapshot);
        data.instance.start();
        _reverbData.AddFirst(data);
    }

    public void RemoveData(ReverbData data)
    {
        data.instance.stop(STOP_MODE.IMMEDIATE);
        data.instance.release();
        _reverbData.Remove(data);

        if (_reverbData.Count > 0)
        {
            var first = _reverbData.First.Value;
            first.instance.setParameterByName("Intensity", 1);
        }
    }
}

[Serializable]
public class ReverbData
{
    public EventReference reverbSnapshot;
    public EventInstance instance;
}
