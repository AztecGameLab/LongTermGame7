using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application.Core;
public class LevelExit : MonoBehaviour
{
    //Contains a string with a target ID for the entrance to use for the scene (where to appear in the next scene)
    public string target_ID;

    //and a string for a the target scene to load.
    public string Next_Scene;
   
    //Attached to a trigger collider, and when a player enters 
    //it fires a LoadLevel event which contains the target ID and scene.
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
        Services.EventBus.Invoke(new LevelChangeEvent{target_ID = target_ID, Next_Scene = Next_Scene}, "LevelExit");
        }        
    }
}
public class LevelChangeEvent{
    //Contains a string with a target ID for the entrance to use for the scene (where to appear in the next scene)
    public string target_ID;

    //and a string for a the target scene to load.
    public string Next_Scene;
    }
