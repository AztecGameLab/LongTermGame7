using System;
using System.Collections.Generic;
using Application.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Associates regions with scenes, for the purpose of sub-arena loading.
/// </summary>
[CreateAssetMenu]
public class SubArenaLookup : ScriptableObject
{
    [SerializeField]
    private List<RegionMappingData> data;
    /// <summary>
    /// Gets the name of a random sub-arena scene, associated with a certain region.
    /// </summary>
    /// <param name="region">The region to look for when choosing a sub-arena scene.</param>
    /// <returns>The name of the scene that contains the sub-arena.</returns>
    public string GetSceneName(RegionTracker.Region region)
    {
        Debug.Log("GetSceneName ENTERED");
        // if you haven't used it yet, you can do this to find the length of a list
        int listLength = data.Count;
        // The unity random utilities will be helpful, especially this one! (first arg is minInclusive, second arg is maxExclusive)
        int randomIndex = Random.Range(0, listLength - 1);
        
        // // and the standard brackets to access elements examples...
        // RegionMappingData firstElement = data[0];
        Debug.Log("ListLength and random Int generator initialized. ABOUT TO ENTER LOOP");
        foreach(var RegionData in data){
            Debug.Log("Loop entered");
            if (region == RegionData.region) {
                Debug.Log("Scene Found!!!");
                
                return RegionData.subArenaSceneNames[randomIndex];
            }
            Debug.Log("If statement skipped!");
        }
        Debug.Log("GetSceneName ENDED");
        return string.Empty;
    }

    /// <summary>
    /// Serialized data for associated a region with a list of sub-arenas.
    /// </summary>
    [Serializable]
    public class RegionMappingData
    {
        public RegionTracker.Region region;
        public string[] subArenaSceneNames;
    }
}