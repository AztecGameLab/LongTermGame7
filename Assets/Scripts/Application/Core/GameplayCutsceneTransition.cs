using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameplayCutsceneTransition : MonoBehaviour
{
    [Serializable]
    public struct KeyValuePair {
        public string key;
        public string val;
    }
 
    public List<KeyValuePair> cutscenes = new List<KeyValuePair>();
    Dictionary<string, string> cutsceneDict = new Dictionary<string, string>();
 
    void Awake() {
        foreach (var kvp in cutscenes) {
        cutsceneDict[kvp.key] = kvp.val;
        }
    }
}
