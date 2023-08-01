using TriInspector;

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

        /// <summary>
        /// Gets or sets the ID of an entrance portal you are aiming for.
        /// </summary>
        [ShowInInspector]
        public string TargetID { get; set; }

        /// <summary>
        /// Gets or sets the name of the scene that this exit will take you to.
        /// </summary>
        [ShowInInspector]
        public string TargetScene { get; set; }

        private void Awake()
        {
            // TargetID = targetID;
            // TargetScene = nextScene;
            ignoreFirst = true;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);
            ignoreFirst = false;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                if (ignoreFirst)
                {
                    ignoreFirst = false;
                    return;
                }

                ignoreFirst = false;

                var e = new LevelChangeEvent
                {
                    SpawningStrategy = new EntranceSpawningStrategy(TargetID),
                    NextScene = TargetScene,
                };

                Services.EventBus.Invoke(e, "LevelExit");
            }
        }
    }
}
