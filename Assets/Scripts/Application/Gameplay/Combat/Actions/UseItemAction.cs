using Cysharp.Threading.Tasks;
using Levels.__TESTING_LEVELS__.Real_Demo;

namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Items;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Vfx;
    using Observable = UniRx.Observable;

    [Serializable]
    public class UseItemAction : BattleAction
    {
        [SerializeField]
        private AssetReference inventoryView;

        [SerializeField]
        private AssetReference hint;

        private BattleInventoryView _view;
        private ItemData _selectedItem;

        /// <inheritdoc/>
        public override string Name => "Use Item";

        /// <inheritdoc/>
        public override string Description => "Perform an action with an item you've found on your adventures!";

        /// <inheritdoc/>
        public override int Cost => 1;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            Inventory inventory = Services.Inventory;
            var exhaustManager = UnityEngine.Object.FindObjectOfType<ExhaustibleManager>();
            _view = Addressables.InstantiateAsync(inventoryView.AssetGUID).WaitForCompletion().GetComponent<BattleInventoryView>();
            _view.ChooseItem(inventory, item =>
            {
                if (!exhaustManager.IsExhausted(item))
                {
                    _selectedItem = item;
                    IsPrepFinished = true;
                }
                else
                {
                    _view.exhaustedPopup.Show().ToUniTask();
                }
            });
        }

        /// <inheritdoc/>
        public override void PrepExit()
        {
            base.PrepExit();

            if (_view != null)
            {
                _view.Close();
            }
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            yield return FuckCoroutines().ToCoroutine().ToYieldInstruction();
        }

        private async UniTask FuckCoroutines()
        {
            if (_selectedItem == null)
            {
                return;
            }

            foreach (IItemEffect itemEffect in _selectedItem.effects)
            {
                await itemEffect.Use().ToUniTask();
            }
        }
    }
}
