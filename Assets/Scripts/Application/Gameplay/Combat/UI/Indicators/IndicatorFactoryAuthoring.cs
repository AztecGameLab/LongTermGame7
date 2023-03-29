namespace Application.Gameplay.Combat.UI.Indicators
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// MonoBehaviour view for setting up an indicator factory.
    /// </summary>
    public class IndicatorFactoryAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ValidityIndicator validityPrefab;

        [SerializeField]
        private PathIndicator pathPrefab;

        [SerializeField]
        private RangeIndicator rangeIndicator;

        [SerializeField]
        private ArrowIndicator arrowIndicator;

        [SerializeField]
        private SliceIndicator sliceIndicator;

        private void Awake()
        {
            var factory = new IndicatorFactory(transform);
            factory.RegisterIndicator(validityPrefab);
            factory.RegisterIndicator(pathPrefab);
            factory.RegisterIndicator(rangeIndicator);
            factory.RegisterIndicator(arrowIndicator);
            factory.RegisterIndicator(sliceIndicator);

            Services.IndicatorFactory = factory;
        }
    }
}
