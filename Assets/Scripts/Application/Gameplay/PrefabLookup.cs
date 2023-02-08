using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Associates prefabs with integer ID's.
/// </summary>
public class PrefabLookup : ScriptableObject
{
    [SerializeField] private List<GameObject> prefabs;

    private Dictionary<GameObject, int> _idLookup;

    public void Init()
    {
        _idLookup = new Dictionary<GameObject, int>();
        
        for (int i = 0; i < prefabs.Count; i++)
        {
            _idLookup.Add(prefabs[i], i);
        }
    }

    public GameObject GetPrefab(int id)
    {
        return prefabs[id];
    }

    public int GetId(GameObject prefab)
    {
        return _idLookup[prefab];
    }
}