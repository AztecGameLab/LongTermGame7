using Application.Gameplay;
using UnityEngine;

// Note: if you haven't used ScriptableObjects before, you need to create an instance of them by right-clicking
// in the project folder view and choosing "Create/SubArenaLookup".

/// <summary>
/// Testing the functionality of the SubArenaLookup
/// </summary>
public class SubArenaTester : MonoBehaviour
{
    [SerializeField]
    private SubArenaLookup lookup;

    [SerializeField]
    private RegionTracker.Region region;

    private void OnEnable()
    {
        lookup.GetSceneName(region);
    }
}