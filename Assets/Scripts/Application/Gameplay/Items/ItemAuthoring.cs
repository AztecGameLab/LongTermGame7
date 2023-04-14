using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Application.Gameplay.Items
{
    [CreateAssetMenu]
    public class ItemAuthoring : ScriptableObject
    {
        [SerializeField]
        private string itemId;

        [SerializeField]
        private string itemName;

        [SerializeField]
        private string itemDescription;

        [SerializeField]
        private AssetReferenceGameObject worldView;

        [SerializeField]
        private AssetReferenceGameObject inventoryView;

        [SerializeReference]
        private IItemEffect[] effects;

        public string ItemName => itemName;

        public string ItemDescription => itemDescription;

        public string ItemId => itemId;

        public ItemData GenerateData()
        {
            return new ItemData
            {
                effects = new ReactiveCollection<IItemEffect>(effects),
                description = new StringReactiveProperty(itemDescription),
                name = new StringReactiveProperty(itemName),
                worldViewAssetPath = new StringReactiveProperty(worldView.AssetGUID),
                inventoryViewAssetPath = new StringReactiveProperty(inventoryView.AssetGUID),
                itemId = itemId,
            };
        }
    }
}
