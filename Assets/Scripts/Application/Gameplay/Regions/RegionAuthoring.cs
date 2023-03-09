namespace Application.Gameplay.Regions
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// Script that should run when a scene loads to set GameData values.
    /// </summary>
    public class RegionAuthoring : MonoBehaviour
    {
        [SerializeField]
        private RegionTracker.Region thisRegion = RegionTracker.Region.Undefined;

        private void Awake()
        {
            if (thisRegion == RegionTracker.Region.Undefined)
            {
                Debug.LogWarning($"This region is undefined! Please change this in this scene's {nameof(RegionAuthoring)}.", gameObject);
            }

            if (Services.RegionTracker != null)
            {
                Services.RegionTracker.CurrentRegion = thisRegion;
            }
        }

        private void OnValidate()
        {
            if (Services.RegionTracker != null)
            {
                Services.RegionTracker.CurrentRegion = thisRegion;
            }
        }
    }
}
