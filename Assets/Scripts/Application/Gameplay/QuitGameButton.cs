using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
