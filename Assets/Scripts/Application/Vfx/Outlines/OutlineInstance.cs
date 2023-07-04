using Application.Core;
using System;
using UnityEngine;

namespace Application.Vfx
{
    /// <summary>
    /// An active outline in the world. Makes it easy to control outline properties and lifetime..
    /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating the current color of the outline.
        /// </summary>
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
}
