namespace Application.Gameplay
{
    using System.Collections;
    using Core;
    using UnityEngine;

    /// <summary>
    /// An area where the player can transition to a different scene.
    /// </summary>
    public class LevelExit : MonoBehaviour
    {
        // Contains a string with a target ID for the entrance to use for the scene (where to appear in the next scene)
        [SerializeField]
        private string targetID;

        // and a string for a the target scene to load.
        [SerializeField]
        private string nextScene;

        [SerializeField]
        private bool ignoreFirst = true;

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            ignoreFirst = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ignoreFirst)
            {
                ignoreFirst = false;
                return;
            }

            if (other.CompareTag("Player"))
            {
                Services.EventBus.Invoke(
                    new LevelChangeEvent { TargetID = targetID, NextScene = nextScene }, "LevelExit");
            }
        }
    }
}