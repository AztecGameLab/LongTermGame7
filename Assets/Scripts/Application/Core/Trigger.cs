namespace Application.Core
{
    using System;
    using UnityEngine;

    /// <summary>
    /// An object that tracks when other objects enter its collision.
    /// </summary>
    public abstract class Trigger : MonoBehaviour
    {
        /// <summary>
        /// Called when an object enters this trigger.
        /// </summary>
        public abstract event Action<GameObject> CollisionEnter;

        /// <summary>
        /// Called when an object exits this trigger.
        /// </summary>
        public abstract event Action<GameObject> CollisionExit;

        private static bool CsgFixed { get; set; }

        private void Awake()
        {
            if (!CsgFixed)
            {
                var fix = new GameObject("CSG Fix");
                Destroy(fix);
                CsgFixed = true;
            }
        }

        private void OnDestroy()
        {
            CsgFixed = false;
        }
    }
}
