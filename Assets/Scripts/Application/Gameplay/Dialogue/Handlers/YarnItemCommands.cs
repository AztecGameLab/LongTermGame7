using Application.Core;
using Application.Gameplay.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Application.Gameplay.Dialogue.Handlers
{
    [Serializable]
    public class YarnItemCommands : IYarnCommandHandler
    {
        [SerializeField]
        private List<ItemAuthoring> items;

        private DialogueRunner _runner;

        public void RegisterCommands(DialogueRunner runner)
        {
            _runner = runner;

            runner.AddCommandHandler<string>("item-add", HandleItemAdd);
            runner.AddCommandHandler<string>("item-remove", HandleItemRemove);
            runner.AddCommandHandler<string>("item-check", HandleItemCheck);
        }

        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("item-add");
            runner.RemoveCommandHandler("item-remove");
            runner.RemoveCommandHandler("item-check");
        }

        private void HandleItemAdd(string itemId)
        {
            foreach (ItemAuthoring item in items)
            {
                if (item.ItemId == itemId)
                {
                    Services.Inventory.Items.Add(item.GenerateData());
                    return;
                }
            }
        }

        private void HandleItemCheck(string itemId)
        {
            var inventory = Services.Inventory;
            bool hasItem = false;

            for (int i = 0; i < inventory.Items.Count; i++)
            {
                if (inventory.Items[i].itemId == itemId)
                {
                    hasItem = true;
                }
            }

            _runner.VariableStorage.SetValue("$hasItem", hasItem);
        }

        private void HandleItemRemove(string itemId)
        {
            var inventory = Services.Inventory;

            for (int i = 0; i < inventory.Items.Count; i++)
            {
                if (inventory.Items[i].itemId == itemId)
                {
                    inventory.Items.Remove(inventory.Items[i]);
                    return;
                }
            }
        }
    }
}
