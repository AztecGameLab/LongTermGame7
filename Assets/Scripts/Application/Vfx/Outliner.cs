using Application.Core;
using System;

namespace Application.Vfx
{
    using System.Collections.Generic;
    using ElRaccoone.Tweens;
    using UnityEngine;

    public class OutlineInstance : IDisposable
    {
        private readonly Material _material;
        private readonly GameObject _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlineInstance"/> class.
        /// </summary>
        /// <param name="material">The material this outline uses.</param>
        /// <param name="source">The object this outline is bound to.</param>
        public OutlineInstance(Material material, GameObject source)
        {
            _material = material;
            _source = source;
        }

        public Color Color
        {
            get => _material.color;
            set => _material.color = value;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Services.Outliner.RemoveOutline(_source);
        }
    }

    /// <summary>
    /// Creates outlines for GameObjects.
    /// </summary>
    public class Outliner
    {
        private readonly Material _outlineMaterial;
        private readonly Dictionary<GameObject, SavedRenderState> _targetsToOutlines;

        /// <summary>
        /// Initializes a new instance of the <see cref="Outliner"/> class.
        /// </summary>
        /// <param name="outlineMaterial">The material used to draw outlines.</param>
        public Outliner(Material outlineMaterial)
        {
            _targetsToOutlines = new Dictionary<GameObject, SavedRenderState>();
            _outlineMaterial = outlineMaterial;
        }

        public void AddOutline(GameObject target, float duration = 0.5f)
        {
            AddOutline(target, Color.white);
        }

        public OutlineInstance AddOutline(GameObject target, Color color, float duration = 0.5f)
        {
            if (!_targetsToOutlines.ContainsKey(target))
            {
                var targetRenderer = target.GetComponentInChildren<SpriteRenderer>();
                var state = new SavedRenderState { Material = targetRenderer.material };
                float from = targetRenderer.material.HasProperty("_Color") ? targetRenderer.material.color.a : 0;
                targetRenderer.material = _outlineMaterial;
                targetRenderer.material.color = color;
                targetRenderer.TweenCancelAll();
                targetRenderer.TweenValueFloat(color.a, duration, value =>
                {
                    var c = targetRenderer.material.color;
                    c.a = value;
                    targetRenderer.material.color = c;
                }).SetFrom(from);

                _targetsToOutlines.Add(target, state);
                return new OutlineInstance(targetRenderer.material, target);
            }

            return null;
        }

        public void RemoveOutline(GameObject target, float duration = 0.5f)
        {
            if (_targetsToOutlines.TryGetValue(target, out SavedRenderState state))
            {
                var renderer = target.GetComponentInChildren<SpriteRenderer>();
                var from = renderer.material.color.a;
                renderer.TweenCancelAll();
                renderer.TweenValueFloat(0, duration, value =>
                {
                    var color = renderer.material.color;
                    color.a = value;
                    renderer.material.color = color;
                }).SetFrom(from).SetOnComplete(() => renderer.material = state.Material);

                _targetsToOutlines.Remove(target);
            }
        }

        private struct SavedRenderState
        {
            public Material Material;
        }
    }
}
