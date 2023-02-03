using System;
using UnityEngine;

namespace Application.Core
{
    public abstract class SerializedBehavior : MonoBehaviour, ISerializable
    {
        [SerializeField, HideInInspector] 
        private string id = Guid.NewGuid().ToString();
        
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

        public abstract string GetID();
        public abstract void ReadData(object data);
        public abstract object WriteData();
    }
}