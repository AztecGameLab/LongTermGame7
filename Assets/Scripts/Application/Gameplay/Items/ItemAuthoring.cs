using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Application.Gameplay.Items
{
    [CreateAssetMenu]
    public class ItemAuthoring : ScriptableObject
    {
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

        public ItemData GenerateData()
        {
            return new ItemData
            {
                effects = new ReactiveCollection<IItemEffect>(effects),
                description = new StringReactiveProperty(itemDescription),
                name = new StringReactiveProperty(itemName),
                worldViewAssetPath = new StringReactiveProperty(worldView.AssetGUID),
                inventoryViewAssetPath = new StringReactiveProperty(inventoryView.AssetGUID),
            };
        }
    }
}