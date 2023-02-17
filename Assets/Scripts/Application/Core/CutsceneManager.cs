using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn.Unity;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] DialogueRunner runner;

    [Header("Object References")]
    public Image bgImage;

    [Serializable]
    public struct KeyValuePair {
        public string key;
        public Sprite val;
    }

    [Tooltip("Manually assign cutscenes here")]
    public List<KeyValuePair> cutscenes = new List<KeyValuePair>();
    Dictionary<string, Sprite> cutsceneDict = new Dictionary<string, Sprite>();
 
    void Awake() {
        //assign the list to dictionary
        foreach (var kvp in cutscenes) {
        cutsceneDict[kvp.key] = kvp.val;
        }

        //Yarn command handlers
        runner.AddCommandHandler<string>("Scene", SceneChange);
        runner.AddCommandHandler("ClearScene", ClearScene);
        runner.AddCommandHandler("LoadScene", LoadScene);
    }

    #region YarnCommands
    public void SceneChange(string cutsceneName) {
        bgImage.sprite = FetchAsset<Sprite>(cutsceneName);
    }

    public void LoadScene() {
        bgImage.enabled = true;
    }

    public void ClearScene() {
        bgImage.sprite = null;
        bgImage.enabled = false;
    }
    #endregion

    //Helper Function for SceneChange
    T FetchAsset<T>(string cutsceneName) where T: UnityEngine.Object {
        if (typeof(T) == typeof(Sprite)) {
            if (cutsceneDict.ContainsKey(cutsceneName)) {
                return cutsceneDict[cutsceneName] as T;
            }
        }
        Debug.LogErrorFormat(this, "Could not find asset. Maybe it was misspelled or never imported?");
        return null;
    }
}

