namespace Application.Gameplay
{
    using Core;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Starts the game when this button is pressed.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private static void HandleButtonClick()
        {
            var message = new StartGameCommand();
            Services.EventBus.Invoke(message, "Start Game Button");
        }

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => HandleButtonClick());
        }
    }
}
