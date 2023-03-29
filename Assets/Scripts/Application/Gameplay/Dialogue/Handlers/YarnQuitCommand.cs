namespace Application.Gameplay.Dialogue.Handlers
{
    using JetBrains.Annotations;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// A yarn command that quits the game.
    /// </summary>
    public static class YarnQuitCommand
    {
        [UsedImplicitly]
        [YarnCommand("quit-game")]
        private static void QuitGame()
        {
            Debug.Log("Game Exit");
            Application.Quit();
        }
    }
}
