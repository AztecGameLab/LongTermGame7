using Application.Core;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Application.Gameplay.Items
{
    public class InventoryView : View<Inventory>
    {
        [SerializeField] private ItemViewUI itemViewPrefab;
        [SerializeField] private Transform itemGroupParent;

        private Dictionary<ItemData, List<ViewInstanceData>> _viewLookup = new Dictionary<ItemData, List<ViewInstanceData>>();
        private Subject<ItemData> _itemClicked = new Subject<ItemData>();

        public IObservable<ItemData> ItemClicked => _itemClicked;

        public override void BindTo(Inventory target)
        {
            base.BindTo(target);

            foreach (ItemData itemData in target.Items)
            {
                HandleItemAdd(itemData);
            }

            target.Items.ObserveAdd().Subscribe(eventData => HandleItemAdd(eventData.Value));
            target.Items.ObserveRemove().Subscribe(eventData => HandleItemRemove(eventData.Value));
        }

        private void HandleItemAdd(ItemData data)
        {
            var instance = Instantiate(itemViewPrefab, itemGroupParent);
            instance.BindTo(data);

            if (!_viewLookup.ContainsKey(data))
            {
                _viewLookup.Add(data, new List<ViewInstanceData>());
            }

            var instanceData = new ViewInstanceData
            {
                Instance = instance,
                OnClickedDisposable = instance.OnPointerClickAsObservable()
                    .Subscribe(_ => _itemClicked.OnNext(data)),
            };

            _viewLookup[data].Add(instanceData);

        }

        private void HandleItemRemove(ItemData data)
        {
            ViewInstanceData instanceData = _viewLookup[data][0];
            Destroy(instanceData.Instance.gameObject);
            instanceData.OnClickedDisposable.Dispose();
            _viewLookup[data].Remove(instanceData);
        }

        private class ViewInstanceData
        {
            public ItemViewUI Instance;
            public IDisposable OnClickedDisposable;
        }
    }
}
