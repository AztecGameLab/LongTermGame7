using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PlayCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
