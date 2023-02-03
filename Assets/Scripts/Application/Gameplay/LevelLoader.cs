using Application.Core;
using Application.Gameplay;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LevelLoader
{
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
        LevelEntrance[] allEntrances = Object.FindObjectsOfType<LevelEntrance>();
        LevelEntrance targetEntrance = null;
        LevelEntrance defaultEntrance = null;

        // Then, runs a loop to see which entrance to use.
        // Once and entrance is found, position the player on the entrance
        foreach (LevelEntrance entrance in allEntrances)
        {
            if (entrance.Entrance_ID == data.target_ID)
            {
                targetEntrance = entrance;
                break;
            }

            if (entrance.default_entrance)
            {
                defaultEntrance = entrance;
            }
        }

        if (targetEntrance == null && defaultEntrance != null)
        {
            targetEntrance = defaultEntrance;
        }
        else if (targetEntrance == null && allEntrances.Length > 0)
        {
            targetEntrance = allEntrances[0];
        }
        else if (targetEntrance == null)
        {
            throw new Exception("No entrances!");
        }
        
        PlayerMovement playerInfo = Object.FindObjectOfType<PlayerMovement>();
        playerInfo.transform.position = targetEntrance.transform.position;
    }
}