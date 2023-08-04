using Application.Core;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Application.Gameplay.Combat.Actions
{
    using Core;
    using UnityEngine;
    using Vfx;
    using Yarn.Unity;

    // Script will regenarate health value to maximum value to player and team composition.
    public class HealthRefilling : MonoBehaviour, IInteractable
    {
        [SerializeField] private LivingEntity entity;

        [SerializeField]
        private DialogueReference reference;

        [SerializeField] private bool oneShot;
        private bool _usable = true;

        public void Start()
        {
            var hintView = GetComponentInChildren<HintView>(true);

            if (hintView != null)
            {
                hintView.gameObject.SetActive(true);
            }
        }

        /// <inheritdoc/>
        public void Interact(GameObject source)
        {
            entity.Heal(entity.MaxHealth - entity.Health);
            if (oneShot && !_usable)
            {
                return;
            }

            _usable = false;
            StartCoroutine(Services.DialogueSystem.RunDialogue(reference));
        }


    }
}