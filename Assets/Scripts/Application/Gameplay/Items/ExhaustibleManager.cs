using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Application.Gameplay.Items
{
    public class ExhaustibleManager : MonoBehaviour
    {
        public List<ExhaustibleEffect> ExhaustedEffects = new List<ExhaustibleEffect>();

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
