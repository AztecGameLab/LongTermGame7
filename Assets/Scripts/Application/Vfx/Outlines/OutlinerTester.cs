using Application.Core.Utility;

namespace Application.Vfx
{
    using Core;
    using JetBrains.Annotations;
    using TriInspector;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// A helper class for testing outline functionality.
    /// </summary>
    public class OutlinerTester : MonoBehaviour
    {
        [SerializeField]
        private Color color = Color.white;

        [SerializeField]
        private float duration = 0.1f;

        private bool _hasOutline;
        private OutlineInstance _outline;

        private void OnValidate()
        {
            if (_hasOutline)
            {
                _outline.Color = color;
            }
        }

        private void Start()
        {
            InputTools.ObjectMouseHover
                .DistinctUntilChanged()
                .Where(obj => obj.GetGameObject() == gameObject && !_hasOutline)
                .Subscribe(_ => ShowOutliner())
                .AddTo(this);

            InputTools.ObjectMouseHover
                .DistinctUntilChanged()
                .Where(obj => obj.GetGameObject() != gameObject && _hasOutline)
                .Subscribe(_ => HideOutliner())
                .AddTo(this);
        }

        [Button]
        [ShowInPlayMode]
        [UsedImplicitly]
        private void ShowOutliner()
        {
            _outline = Services.Outliner.AddOutline(gameObject, color, duration);
            _hasOutline = true;
        }

        [Button]
        [ShowInPlayMode]
        [UsedImplicitly]
        private void HideOutliner()
        {
            Services.Outliner.RemoveOutline(gameObject, duration);
            _hasOutline = false;
        }
    }
}
