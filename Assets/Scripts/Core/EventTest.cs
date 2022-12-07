using UnityEngine;

namespace DefaultNamespace.Core
{
    [CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/Event Test")]
    public class EventTest : Event<int>
    {
    }
}