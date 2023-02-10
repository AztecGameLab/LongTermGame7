namespace Application.Gameplay.Landmarks
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A special location in a level.
    /// </summary>
    public class Landmark : MonoBehaviour
    {
        private static List<Landmark> _landmarks;

        [SerializeField]
        [Tooltip("A human-readable way to identify this landmark.")]
        private string id;

        /// <summary>
        /// Gets all of the currently loaded landmarks in the scene.
        /// </summary>
        /// <value>
        /// All of the currently loaded landmarks in the scene.
        /// </value>
        public static IReadOnlyList<Landmark> Landmarks => _landmarks;

        /// <summary>
        /// Gets a human-readable way to identify this landmark.
        /// </summary>
        public string Id
        {
            get => id;
            set => id = value;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            _landmarks = new List<Landmark>();
        }

        private void OnEnable()
        {
            _landmarks.Add(this);
        }

        private void OnDisable()
        {
            _landmarks.Remove(this);
        }
    }
}
