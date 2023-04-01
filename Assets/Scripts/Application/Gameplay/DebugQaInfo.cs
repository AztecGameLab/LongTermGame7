namespace Application.Gameplay
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Prints debugging information for QA testing.
    /// </summary>
    public class DebugQaInfo : MonoBehaviour
    {
        private const int BuildNumber = 2;

        [SerializeField]
        private TMP_Text textDisplay;

        private void Update()
        {
            textDisplay.text = $"Build {BuildNumber} : {SceneManager.GetActiveScene().name}";
        }
    }
}
