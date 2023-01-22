using Application.Core;

namespace Application.Vfx.Animation
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Stores a set of materials that are associated with directions.
    /// </summary>
    [Serializable]
    public class MovementMaterialSet
    {
        private readonly Material[] _materialArray = new Material[4];

        [SerializeField]
        private Material up;

        [SerializeField]
        private Material down;

        [SerializeField]
        private Material left;

        [SerializeField]
        private Material right;

        /// <summary>
        /// Re-constructs internal state.
        /// <remarks>Should be called whenever the serialized data changes.</remarks>
        /// </summary>
        public void Build()
        {
            Set(MovementDirection.Up, up);
            Set(MovementDirection.Down, down);
            Set(MovementDirection.Left, left);
            Set(MovementDirection.Right, right);
        }

        /// <summary>
        /// Looks up a material associated with a movement direction.
        /// </summary>
        /// <param name="direction">The direction to use when retrieving.</param>
        /// <returns>The material associated with the direction.</returns>
        [CanBeNull]
        public Material Get(MovementDirection direction)
        {
            return _materialArray[(int)direction];
        }

        private void Set(MovementDirection direction, Material material)
        {
            _materialArray[(int)direction] = material;
        }
    }
}
