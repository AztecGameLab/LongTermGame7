namespace Application.Core
{
    using UnityEngine;

    public class QuitGameLogic : MonoBehaviour
    {
        public void QuitGame()
        {
            Debug.Log("Quit game");
            Application.Quit();
        }
    }
}
