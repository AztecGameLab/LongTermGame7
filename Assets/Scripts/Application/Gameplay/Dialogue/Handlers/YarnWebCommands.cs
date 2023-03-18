namespace Application.Gameplay.Dialogue.Handlers
{
    using JetBrains.Annotations;
    using Yarn.Unity;

    /// <summary>
    /// Commands for opening websites through yarn.
    /// </summary>
    public static class YarnWebCommands
    {
        [UsedImplicitly]
        [YarnCommand("web-open")]
        private static void OpenWebsite(string website)
        {
            // <<web-open google.com>>
            UnityEngine.Application.OpenURL(website);
        }
    }
}
