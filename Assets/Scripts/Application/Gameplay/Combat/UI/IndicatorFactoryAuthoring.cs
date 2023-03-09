namespace Application.Gameplay.Combat
{
    using Core;
    using UI;
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

        private void Awake()
        {
            var factory = new IndicatorFactory();
            factory.RegisterIndicator(validityPrefab);
            factory.RegisterIndicator(pathPrefab);
            factory.RegisterIndicator(rangeIndicator);

            Services.IndicatorFactory = factory;
        }
    }
}
