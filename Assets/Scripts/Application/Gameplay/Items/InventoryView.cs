using Application.Core;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Items
{
    public class InventoryView : View<Inventory>
    {
        [SerializeField] private ItemViewUI itemViewPrefab;
        [SerializeField] private Transform itemGroupParent;

        private Dictionary<ItemData, List<ItemViewUI>> _viewLookup = new Dictionary<ItemData, List<ItemViewUI>>();

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
                _viewLookup.Add(data, new List<ItemViewUI>());
            }

            _viewLookup[data].Add(instance);

        }

        private void HandleItemRemove(ItemData data)
        {
            ItemViewUI instance = _viewLookup[data][0];
            Destroy(instance);
            _viewLookup[data].Remove(instance);
        }
    }
}
