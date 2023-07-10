﻿using Application.Core;
using System;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Items
{
    public class InventoryLoader : MonoBehaviour
    {
        private const string InventoryID = "player_inventory";

        [SerializeField] private ItemAuthoring[] testingItems;
        [SerializeField] private InventoryView inventoryView;

        private void Awake()
        {
            var serializer = Services.Serializer;

            serializer.ObserveRead().Subscribe(_ => HandleRead()).AddTo(this);
            serializer.ObserveWrite().Subscribe(_ => HandleWrite()).AddTo(this);

            HandleRead();

            Services.Inventory.Items.ObserveAdd().Subscribe(item => HandleNewItem(item.Value)).AddTo(this);
            inventoryView.BindTo(Services.Inventory);
            inventoryView.ItemClicked.Subscribe(data =>
            {
                foreach (IItemEffect itemEffect in data.effects)
                {
                    StartCoroutine(itemEffect.Use());
                }
            });
        }

        private void HandleNewItem(ItemData item)
        {
            foreach (IItemEffect effect in item.effects)
            {
                effect.Initialize();
            }
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.I))
            //     inventoryView.gameObject.SetActive(!inventoryView.gameObject.activeSelf);
        }

        private void HandleRead()
        {
            var serializer = Services.Serializer;

            if (!serializer.TryLookup(InventoryID, out Inventory inventory))
            {
                inventory = new Inventory();

                foreach (ItemAuthoring item in testingItems)
                {
                    var data = item.GenerateData();
                    inventory.Items.Add(data);
                }
            }

            foreach (ItemData item in inventory.Items)
            {
                foreach (IItemEffect effect in item.effects)
                {
                    effect.Initialize();
                }
            }

            Services.Inventory = inventory;
        }

        private void HandleWrite()
        {
            Services.Serializer.Store(InventoryID, Services.Inventory);
        }
    }
}
