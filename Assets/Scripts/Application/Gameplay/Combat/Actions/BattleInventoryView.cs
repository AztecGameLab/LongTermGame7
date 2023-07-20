using Application.Gameplay.Items;
using Levels.__TESTING_LEVELS__.Real_Demo;
using System;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat.Actions
{
    public class BattleInventoryView : MonoBehaviour
    {
        public InventoryView view;
        public ClickToClearPopup exhaustedPopup;

        public void ChooseItem(Inventory inventory, Action<ItemData> onPick)
        {
            view.BindTo(inventory);
            view.ItemClicked.Subscribe(onPick.Invoke);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
