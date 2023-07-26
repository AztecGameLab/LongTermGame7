namespace Application.Gameplay.Items
{
    using System.Collections.Generic;
    using TriInspector;
    using UnityEngine;

    public class ExhaustibleManager : MonoBehaviour
    {
        public List<ExhaustibleEffect> ExhaustedEffects = new List<ExhaustibleEffect>();

        public bool IsExhausted(ItemData item)
        {
            // I'm exhausted too, hence we do it like this.
            foreach (IItemEffect itemEffect in item.effects)
            {
                foreach (ExhaustibleEffect effect in ExhaustedEffects)
                {
                    if (itemEffect == effect)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [Button]
        public void RestoreAll()
        {
            foreach (ExhaustibleEffect effect in ExhaustedEffects)
            {
                effect.isExhausted = false;
            }

            ExhaustedEffects.Clear();
        }
    }
}
