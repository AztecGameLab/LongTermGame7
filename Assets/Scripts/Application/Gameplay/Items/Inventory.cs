using Newtonsoft.Json;
using UniRx;

namespace Application.Gameplay.Items
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Inventory
    {
        [JsonProperty]
        public ReactiveCollection<ItemData> Items { get; }
            = new ReactiveCollection<ItemData>();
    }
}