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
        private GameObject cubePrefab;

        private void Awake()
        {
            Services.IndicatorFactory = new IndicatorFactory(cubePrefab);
        }
    }
}
