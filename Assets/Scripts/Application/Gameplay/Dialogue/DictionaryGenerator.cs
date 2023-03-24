namespace Application.Gameplay.Dialogue
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A inspector-friendly generator for dictionaries.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    [Serializable]
    public class DictionaryGenerator<TKey, TValue>
    {
        [SerializeField]
        private List<KeyValuePair> dictionary;

        /// <summary>
        /// Creates a new dictionary from the inspector-defined values.
        /// </summary>
        /// <returns>A dictionary filled with inspector-defined values.</returns>
        public Dictionary<TKey, TValue> GenerateDictionary()
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair keyValuePair in dictionary)
            {
                result.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return result;
        }

        [Serializable]
        private sealed class KeyValuePair
        {
            [SerializeField]
            private TKey key;

            [SerializeField]
            private TValue value;

            public TKey Key => key;

            public TValue Value => value;
        }
    }
}
