namespace Application.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Associates prefabs with integer ID's.
    /// </summary>
    public class PrefabLookup : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> prefabs;

        private Dictionary<GameObject, int> _idLookup;

        /// <summary>
        /// Sets up the prefab lookup.
        /// </summary>
        public void Init()
        {
            _idLookup = new Dictionary<GameObject, int>();

            for (int i = 0; i < prefabs.Count; i++)
            {
                _idLookup.Add(prefabs[i], i);
            }
        }

        /// <summary>
        /// Finds a prefab associated with an id.
        /// </summary>
        /// <param name="id">The id to use when looking for the prefab.</param>
        /// <returns>The prefab associated with the id.</returns>
        public GameObject GetPrefab(int id)
        {
            return prefabs[id];
        }

        /// <summary>
        /// Gets the id associated with a prefab.
        /// </summary>
        /// <param name="prefab">The prefab to use when looking for an id.</param>
        /// <returns>The id associated with the given prefab.</returns>
        public int GetId(GameObject prefab)
        {
            return _idLookup[prefab];
        }
    }
}
