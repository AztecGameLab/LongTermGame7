using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class MoveListUI : View<IEnumerable<BattleAction>>
    {
        [SerializeField] private Transform listTarget;
        [SerializeField] private MoveUI moveUI;
        
        public override void BindTo(IEnumerable<BattleAction> target)
        {
            foreach (BattleAction action in target)
            {
                var instance = Instantiate(moveUI, listTarget);
                instance.BindTo(action);
            }
        }
    }
}