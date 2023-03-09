namespace Application.Core.Serialization
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Represents some MonoBehavior that is automatically saved and loaded while in a scene.
    /// </summary>
    public abstract class SerializedBehavior : MonoBehaviour, ISerializable
    {
        [SerializeField]
        [HideInInspector]
        [UsedImplicitly]
        private string id = Guid.NewGuid().ToString();

        /// <inheritdoc/>
        public abstract string Id { get; }

        /// <inheritdoc/>
        public abstract void ReadData(object data);

        /// <inheritdoc/>
        public abstract object WriteData();

        private void Awake()
        {
            Services.Serializer.ApplySavedDataTo(this);
        }

        private void OnDestroy()
        {
            Services.Serializer.UpdateSavedDataFrom(this);
        }

        private void OnApplicationQuit()
        {
            Services.Serializer.UpdateSavedDataFrom(this);
        }
    }
}
