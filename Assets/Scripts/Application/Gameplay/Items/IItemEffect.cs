using Application.Core;
using Application.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Items
{
    public interface IItemEffect
    {
        IEnumerator Use();
    }

    [Serializable]
    public class HealAllEffect : IItemEffect
    {
        [SerializeField]
        private float amount;

        public IEnumerator Use()
        {
            Collection<GameObject> playerTeam = Object.FindObjectOfType<BattleController>().PlayerTeam;

            foreach (GameObject memberInstance in playerTeam)
            {
                if (memberInstance.TryGetComponent(out LivingEntity entity))
                    entity.Heal(amount);
            }

            // todo: play some healing sounds and vfx
            yield return new WaitForSeconds(1);
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
