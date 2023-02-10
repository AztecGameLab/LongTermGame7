namespace Application.Vfx.Animation
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Data used for playing an CPU-based animation on a Mesh Renderer.
    /// </summary>
    [CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/CPU Animation Data")]
    public class CpuAnimationData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The images that will be played, in order, for this animation.")]
        private List<Texture2D> frames;

        [SerializeField]
        [Tooltip("How many frames-per-second this animation should run at.")]
        private float frameRate;

        /// <summary>
        /// Gets the images that will be played, in order, for this animation.
        /// </summary>
        /// <value>
        /// The images that will be played, in order, for this animation.
        /// </value>
        public IReadOnlyList<Texture2D> Frames => frames;

        /// <summary>
        /// Gets how many frames-per-second this animation should run at.
        /// </summary>
        /// <value>
        /// How many frames-per-second this animation should run at.
        /// </value>
        public float FrameRate => frameRate;
    }
}
