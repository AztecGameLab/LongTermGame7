namespace Application.Vfx
{
    using System.Collections.Generic;
    using ElRaccoone.Tweens;
    using UnityEngine;

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

        /// <summary>
        /// Add an outline to an object in the world.
        /// This only works for objects with one <see cref="SpriteRenderer"/> in their children.
        /// </summary>
        /// <param name="target">The object to add an outline to.</param>
        /// <param name="duration">The animation speed for the outline appearing.</param>
        /// <returns>An instance of the created outline - can be used for easy control.</returns>
        public OutlineInstance AddOutline(GameObject target, float duration = 0.5f)
        {
            return AddOutline(target, Color.white);
        }

        /// <summary>
        /// Add an outline to an object in the world.
        /// This only works for objects with one <see cref="SpriteRenderer"/> in their children.
        /// </summary>
        /// <param name="target">The object to add an outline to.</param>
        /// <param name="color">The color the outline should initially be.</param>
        /// <param name="duration">The animation speed for the outline appearing.</param>
        /// <returns>An instance of the created outline - can be used for easy control.</returns>
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

        /// <summary>
        /// Removes an outline from an object in the world, if it has one.
        /// </summary>
        /// <param name="target">The object that's outline should be removed.</param>
        /// <param name="duration">The duration of the fading out animation.</param>
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

        /// <summary>
        /// Used for restoring a renderer after removing its outline.
        /// </summary>
        private struct SavedRenderState
        {
            public Material Material;
        }
    }
}
