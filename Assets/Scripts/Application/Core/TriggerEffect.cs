using Application.Core;
using UnityEngine;

namespace Application.Gameplay
{
    [RequireComponent(typeof(Trigger))]
    public abstract class TriggerEffect : MonoBehaviour
    {
        private Trigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
        }

        private void OnEnable()
        {
            _trigger.CollisionEnter += HandleCollisionEnter;
            _trigger.CollisionExit += HandleCollisionExit;
        }

        private void OnDisable()
        {
            _trigger.CollisionEnter -= HandleCollisionEnter;
            _trigger.CollisionExit -= HandleCollisionExit;
        }

        protected virtual void HandleCollisionEnter(GameObject obj) {}

        protected virtual void HandleCollisionExit(GameObject obj) {}
    }
}