using JetBrains.Annotations;
using System;
using UniRx;

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
            InputTools.ObjectMouseOver.Where(obj => obj == gameObject).Subscribe(_ => ShowOutliner());
            InputTools.ObjectMouseOver.Where(obj => obj != gameObject && _hasOutline).Subscribe(_ => HideOutliner());
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
