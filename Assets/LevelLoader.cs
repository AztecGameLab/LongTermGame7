using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Core;
using UnityEngine.SceneManagement;
using Application.Gameplay;
using System.Threading.Tasks;
public class LevelLoader : MonoBehaviour
{
    /* Manager that hooks everything together
    *  by listening for the LoadLevel events
    */
    public void Init()
    {   
        Services.EventBus.AddListener<LevelChangeEvent>(HandleSceneChange, "LevelLoading"); 
    }

    // Once the listener catches an event, it responds with the HandleSceneChange method
    // by passing the data from the LevelChangeEvent
    private async void HandleSceneChange(LevelChangeEvent data)
    {
        // Loads the Next Scene and creates a list of all the entrances in that scene
       SceneManager.LoadScene(data.Next_Scene);
       await Task.Delay(1);
       LevelEntrance[] listOfScenes = FindObjectsOfType<LevelEntrance>();

        // Then, runs a loop to see which entrance to use.
        // Once and entrance is found, position the player on the entrance
       foreach(LevelEntrance entrance in listOfScenes)
       {
        if (entrance.Entrance_ID == data.target_ID) {
            PlayerMovement playerInfo = FindObjectOfType<PlayerMovement>();
            playerInfo.transform.position = entrance.transform.position;
            return;
        }
       }

       // If the entrance is not found, search for a default entrance
       foreach(LevelEntrance entrance in listOfScenes)
       {
        if (entrance.default_entrance)
        {
            PlayerMovement playerInfo = FindObjectOfType<PlayerMovement>();
            playerInfo.transform.position = entrance.transform.position;
            return;
        }

        // If no default entrance is found, displa an error message to let the debugger know
        Debug.LogError("No default entrance!!!");
       }
    }
}
