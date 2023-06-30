using JetBrains.Annotations;

namespace Application.Vfx
{
    using Core;
    using TriInspector;
    using UnityEngine;

    /// <summary>
    /// A helper class for testing outline functionality.
    /// </summary>
    public class OutlinerTester : MonoBehaviour
    {
        [SerializeField]
        private Color color;

        private bool _hasOutline;
        private OutlineInstance _outline;

        private void OnValidate()
        {
            if (_hasOutline)
            {
                _outline.Color = color;
            }
        }

        [Button]
        [ShowInPlayMode]
        [UsedImplicitly]
        private void ShowOutliner()
        {
            _outline = Services.Outliner.AddOutline(gameObject, color);
            _hasOutline = true;
        }

        [Button]
        [ShowInPlayMode]
        [UsedImplicitly]
        private void HideOutliner()
        {
            Services.Outliner.RemoveOutline(gameObject);
            _hasOutline = false;
        }
    }
}
