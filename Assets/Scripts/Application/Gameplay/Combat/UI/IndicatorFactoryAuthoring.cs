namespace Application.Gameplay.Combat
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

        private void Awake()
        {
            var factory = new IndicatorFactory();
            factory.RegisterIndicator(validityPrefab);
            factory.RegisterIndicator(pathPrefab);

            Services.IndicatorFactory = factory;
        }
    }
}
