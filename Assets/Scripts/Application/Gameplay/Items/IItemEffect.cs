using Application.Core;
using Application.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Yarn.Unity;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Items
{
    public interface IItemEffect
    {
        IEnumerator Use();
    }

    [Serializable]
    public class ExhaustibleEffect : IItemEffect
    {
        public bool isExhausted;

        [SerializeReference]
        public List<IItemEffect> effects;

        public IEnumerator Use()
        {
            if (!isExhausted)
            {
                isExhausted = true;
                Object.FindObjectOfType<ExhaustibleManager>().ExhaustedEffects.Add(this);

                foreach (IItemEffect itemEffect in effects)
                {
                    yield return itemEffect.Use();
                }
            }
        }
    }

    [Serializable]
    public class HealAllEffect : IItemEffect
    {
        [SerializeField]
        private float amount;

        [SerializeField] private DialogueReference message;

        public IEnumerator Use()
        {
            Collection<GameObject> playerTeam = Object.FindObjectOfType<BattleController>(true).PlayerTeam;

            foreach (GameObject memberInstance in playerTeam)
            {
                if (memberInstance.TryGetComponent(out LivingEntity entity))
                    entity.Heal(amount);
            }

            yield return Services.DialogueSystem.RunDialogue(message);
        }
    }

    [Serializable]
    public class PrintMessageEffect : IItemEffect
    {
        [SerializeField] private string message;

        public IEnumerator Use()
        {
            Debug.Log(message);
            yield return new WaitForSeconds(2);
        }
    }
}
