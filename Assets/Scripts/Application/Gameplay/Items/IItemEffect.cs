using Application.Core;
using Application.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Items
{
    public interface IItemEffect
    {
        void Initialize();
        IEnumerator Use();
    }

    [Serializable]
    public class ExhaustibleEffect : IItemEffect
    {
        [SerializeReference]
        public List<IItemEffect> effects;

        public bool isExhausted;

        public void Initialize()
        {
            isExhausted = false;
        }

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

        public void Initialize()
        {
        }

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

        public void Initialize()
        {
        }

        public IEnumerator Use()
        {
            Debug.Log(message);
            yield return new WaitForSeconds(2);
        }
    }
    
    [Serializable]
    public class StrengthenAllEffectLow : IItemEffect
    {
        [SerializeField]
        private string size;

        [SerializeField] private DialogueReference message;

        public void Initialize()
        {
        }

        public IEnumerator Use()
        {
            Collection<GameObject> playerTeam = Object.FindObjectOfType<BattleController>(true).PlayerTeam;

            foreach (GameObject memberInstance in playerTeam)
            {
                if (memberInstance.TryGetComponent(out LivingEntity entity))
                    entity.StrengthenRH(size);
            }

            yield return Services.DialogueSystem.RunDialogue(message);
        }
    }

    [Serializable]
    public class StrengthenAllEffectMid : IItemEffect
    {
        [SerializeField] private string size;

        [SerializeField] private DialogueReference message;

        public void Initialize()
        {
        }

        public IEnumerator Use()
        {
            Collection<GameObject> playerTeam = Object.FindObjectOfType<BattleController>(true).PlayerTeam;

            foreach (GameObject memberInstance in playerTeam)
            {
                if (memberInstance.TryGetComponent(out LivingEntity entity))
                    entity.StrengthenRM(size);
            }

            yield return Services.DialogueSystem.RunDialogue(message);
        }
    }

    [Serializable]
    public class StrengthenAllEffectHigh : IItemEffect
    {
        [SerializeField] private string size;
        [SerializeField] private DialogueReference message;
        public void Initialize() {}

        public IEnumerator Use() 
        {
            Collection<GameObject> playerTeam = Object.FindObjectOfType<BattleController>(true).PlayerTeam;
            foreach (GameObject memberInstance in playerTeam) 
            { 
                if (memberInstance.TryGetComponent(out LivingEntity entity)) 
                    entity.StrengthenRE(size);
            }
            
            yield return Services.DialogueSystem.RunDialogue(message);
        }


    }
}
