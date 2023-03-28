using JetBrains.Annotations;
using UnityEngine;
using Yarn.Unity;

namespace Application.Gameplay.Dialogue.Handlers
{
    public static class YarnQuitCommand
    {
        [UsedImplicitly]
        [YarnCommand("quit-game")]
        static void quitGame()
        {
            Debug.Log("Game Exit");
            UnityEngine.Application.Quit();
        }
    }
}