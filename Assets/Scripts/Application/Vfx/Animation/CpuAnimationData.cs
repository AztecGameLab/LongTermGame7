using System.Collections.Generic;
using Application;
using Application.Vfx;
using UnityEngine;

[CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/" + VfxConstants.AssetMenuName + "/CPU Animation Data")]
public class CpuAnimationData : ScriptableObject
{
    [SerializeField]
    [Tooltip("The images that will be played, in order, for this animation.")]
    private List<Texture2D> frames;
     
    [SerializeField]
    [Tooltip("How many frames-per-second this animation should run at.")]
    private float frameRate;

    /// <summary>
    /// The images that will be played, in order, for this animation.
    /// </summary>
    public IReadOnlyList<Texture2D> Frames => frames;
    
    /// <summary>
    /// How many frames-per-second this animation should run at.
    /// </summary>
    public float FrameRate => frameRate;
}