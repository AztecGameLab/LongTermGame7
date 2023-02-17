using Application.Core;
using UnityEngine;

namespace Application.Gameplay
{
    [RequireComponent(typeof(Trigger))]
    public abstract class TriggerEffect : MonoBehaviour
    {
        private void OnEnable()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter += HandleCollisionEnter;
            trigger.CollisionExit += HandleCollisionExit;
        }

        private void OnDisable()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter -= HandleCollisionEnter;
            trigger.CollisionExit -= HandleCollisionExit;
        }

        protected virtual void HandleCollisionEnter(GameObject obj) {}

        protected virtual void HandleCollisionExit(GameObject obj) {}
    }
}