namespace Application.Vfx
{
    using System;
    using System.Collections.Generic;
    using Core;
    using UnityEngine;

    public class ShakeInstance : IDisposable
    {
        public float Magnitude { get; set; } = 1;

        public float Frequency { get; set; } = 1;

        public GameObject Target { get; }

        public Vector3 Position { get; set; }

        private readonly List<ShakeInstance> _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShakeInstance"/> class.
        /// </summary>
        /// <param name="target">The object that is being shaken.</param>
        public ShakeInstance(GameObject target, List<ShakeInstance> parent)
        {
            Target = target;
            _parent = parent;
            _parent.Add(this);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _parent.Remove(this);
            Target.transform.position = Position;
        }
    }

    public class ShakeApplier : MonoBehaviour
    {
        private List<ShakeInstance> _shakeInstances = new List<ShakeInstance>();

        private void Start()
        {
            Services.ShakeApplier = this;
        }

        private void Update()
        {
            foreach (var instance in _shakeInstances)
            {
                var targetTransform = instance.Target.transform;
                float offsetX = (Mathf.PerlinNoise(1, instance.Frequency * Time.time) - 0.5f) * 2 * instance.Magnitude;
                // float offsetY = (Mathf.PerlinNoise(5, instance.Frequency * Time.time) - 0.5f) * 2 * instance.Magnitude;
                float offsetY = 0;
                targetTransform.position = instance.Position + new Vector3(offsetX, offsetY);
            }
        }

        public ShakeInstance ApplyShake(GameObject target, float frequency = 1, float magnitude = 1)
        {
            return new ShakeInstance(target, _shakeInstances)
            {
                Frequency = frequency, Magnitude = magnitude, Position = target.transform.position,
            };
        }
    }
}
