using Application;
using Application.Vfx;
using UnityEngine;

[CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/" + VfxConstants.AssetMenuName + "/Flipbook Animation Data")]
public class FlipbookAnimationData : ScriptableObject
{
    [SerializeField]
    [Tooltip("The material that should be applied for this animation.")]
    private Material animationMaterial;

    /// <summary>
    /// The material that should be applied for this animation.
    /// </summary>
    public Material Material => animationMaterial;
}