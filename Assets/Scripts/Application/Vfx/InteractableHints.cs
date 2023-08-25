using System.Linq;

namespace Application.Vfx
{
    using System.Collections.Generic;
    using Core;
    using Gameplay;
    using UnityEngine;

    /// <summary>
    /// Causes hints to appear when they enter this trigger.
    /// </summary>
    public class InteractableHints : TriggerEffect
    {
        private readonly Dictionary<GameObject, HintView> _interactablesToHints = new Dictionary<GameObject, HintView>();

        [SerializeField]
        private HintView defaultHint;

        public IInteractable GetNearest(Vector3 position)
        {
            GameObject nearest = _interactablesToHints.Keys
                .OrderBy(key => (position - key.transform.position).sqrMagnitude)
                .FirstOrDefault();

            if (nearest != null)
            {
                return nearest.GetComponentInChildren<IInteractable>();
            }

            return null;
        }

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            if (_interactablesToHints.ContainsKey(obj))
            {
                return;
            }

            var hintView = obj.GetComponentInChildren<HintView>();

            // If we are interactable, we want to create a default hint
            if (hintView == null && obj.GetComponentInChildren<IInteractable>() != null)
            {
                hintView = Instantiate(defaultHint, obj.transform);
                hintView.transform.position += Vector3.up * 2;
            }

            if (hintView != null)
            {
                hintView.Show();
                _interactablesToHints.Add(obj, hintView);
            }
        }

        /// <inheritdoc/>
        protected override void HandleCollisionExit(GameObject obj)
        {
            if (_interactablesToHints.TryGetValue(obj, out HintView hint))
            {
                // we want to play the hint removing animation
                hint.Hide();
                _interactablesToHints.Remove(obj);
            }
        }
    }
}
