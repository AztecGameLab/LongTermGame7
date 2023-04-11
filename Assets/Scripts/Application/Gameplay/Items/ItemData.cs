using Application.Core.Serialization;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Application.Gameplay.Items
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemData
    {
        [JsonProperty]
        public StringReactiveProperty name;

        [JsonProperty]
        public StringReactiveProperty description;

        [JsonProperty]
        public StringReactiveProperty worldViewAssetPath;

        [JsonProperty]
        public StringReactiveProperty inventoryViewAssetPath;

        [JsonProperty]
        public ReactiveCollection<IItemEffect> effects;
    }
}
