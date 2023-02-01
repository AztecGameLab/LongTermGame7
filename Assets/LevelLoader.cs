using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Core;
using UnityEngine.SceneManagement;
using Application.Gameplay;
using System.Threading.Tasks;
public class LevelLoader : MonoBehaviour
{
    public void Init()
    {
        Services.EventBus.AddListener<LevelChangeEvent>(HandleSceneChange, "LevelLoading");
    }
    private async void HandleSceneChange(LevelChangeEvent data)
    {
       SceneManager.LoadScene(data.Next_Scene);
        await Task.Delay(1);
       LevelEntrance[] listOfScenes = FindObjectsOfType<LevelEntrance>();

       foreach(LevelEntrance entrance in listOfScenes)
       {
        if (entrance.Entrance_ID == data.target_ID) {
            PlayerMovement playerInfo = FindObjectOfType<PlayerMovement>();
            playerInfo.transform.position = entrance.transform.position;
            return;
        }
       }
       foreach(LevelEntrance entrance in listOfScenes)
       {
        if (entrance.default_entrance)
        {
            PlayerMovement playerInfo = FindObjectOfType<PlayerMovement>();
            playerInfo.transform.position = entrance.transform.position;
            return;
        }

        Debug.LogError("No default entrance!!!");
       }
    }
}
