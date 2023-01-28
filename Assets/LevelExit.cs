using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //Contains a string with a target ID for the entrance to use for the scene (where to appear in the next scene)
    private string target_ID;

    //and a string for a the target scene to load.
    private string Next_Scene;
   
    //Attached to a trigger collider, and when a player enters 
    //it fires a LoadLevel event which contains the target ID and scene.
    void OnTriggerEnter(Collider other) {}
}
