using Application.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Application.Gameplay.Items
{
    public class ItemViewUI : View<ItemData>
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform viewParent;

        private GameObject _viewInstance;

        public override void BindTo(ItemData target)
        {
            base.BindTo(target);

            nameText.text = target.name.Value;
            descriptionText.text = target.description.Value;
            HandleViewChanged(target.inventoryViewAssetPath.Value);

            target.name.SubscribeToText(nameText).AddTo(this);
            target.description.SubscribeToText(descriptionText).AddTo(this);
            target.inventoryViewAssetPath.Subscribe(data => HandleViewChanged(data)).AddTo(this);
        }

        private void HandleViewChanged(string newViewPath)
        {
            if (_viewInstance != null)
            {
                Destroy(_viewInstance);
            }

            if (newViewPath != string.Empty)
                _viewInstance = Addressables.InstantiateAsync(newViewPath, viewParent).WaitForCompletion();
        }
    }
}
