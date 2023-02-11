using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTest : MonoBehaviour
{
    [Serializable]
    public struct KeyValue
    {
        public string Key;
        public EventReference Event;
    }

    public List<KeyValue> Sfx;

    private Dictionary<string, EventReference> _sfx = new Dictionary<string, EventReference>();

    [YarnCommand("play_sfx")]
    public void PlayAudio(string name)
    {
        RuntimeManager.PlayOneShot(_sfx[name]);
        Debug.Log("Played audio!");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (KeyValue v in Sfx)
        {
            _sfx.Add(v.Key, v.Event);
        }
    }

}
