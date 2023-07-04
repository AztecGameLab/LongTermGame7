namespace Application.Gameplay.Dialogue
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// A template for an object that knows how to play an emote.
    /// </summary>
    public abstract class EmotePlayer : MonoBehaviour
    {
        /// <summary>
        /// Play an animation for showing an emote.
        /// </summary>
        /// <param name="target">The object that the emote should originate from.</param>
        /// <returns>A unity coroutine.</returns>
        public Coroutine Play(GameObject target)
        {
            return StartCoroutine(AnimationCoroutine(target));
        }

        /// <summary>
        /// Play an animation for showing an emote.
        /// </summary>
        /// <param name="target">The object that the emote should originate from.</param>
        /// <returns>Coroutine enumerator.</returns>
        protected abstract IEnumerator AnimationCoroutine(GameObject target);
    }
}
