using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    // if (other.CompareTag("Player"))
    // {
    //     SceneManager.LoadScene(Next_Scene);
    // }

// Finally, we need a persistent manager to hook everything together 
// by listening for the LoadLevel events and responding. You can add 
// this to the Entrypoint and add the data it needs to the ApplicationSettings.

// When the LevelLoader recieves a LoadLevel event, it loads the desired scene, 
// then FindObjectOfType searches through LevelEntrance's for one that matches the target ID.

// If none are found, use the first one that is "default entrance" and Debug log a warning.
    // if () {
        
    //}
// If none are the "default entrance", then just use the first one and Debug log a warning.

// If no entrances are found, then just throw an exception.

// Finally, once we have the scene loaded and our entrance found, instantiate the player prefab and change its transform to the entrance.
   
   

}
