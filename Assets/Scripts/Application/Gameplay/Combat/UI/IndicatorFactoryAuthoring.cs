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
            Services.IndicatorFactory = new IndicatorFactory(validityPrefab, pathPrefab);
        }
    }
}
