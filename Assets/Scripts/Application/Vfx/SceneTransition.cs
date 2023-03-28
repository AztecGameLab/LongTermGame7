namespace Application.Vfx
{
    using System;
    using System.Collections;
    using UniRx;

    /// <summary>
    /// A transition that can be used to hide the contents of the screen.
    /// </summary>
    public abstract class SceneTransition
    {
        /// <summary>
        /// Shows this transition effect on the screen, usually ending
        /// when the game-world is fully obscured.
        /// </summary>
        /// <returns>An observable that ends when the effect has finished.</returns>
        public IObservable<Unit> ShowEffect() => ShowEffectCoroutine().ToObservable();

        /// <summary>
        /// Removes this transition effect from the screen, usually ending
        /// when the game-world is fully visible.
        /// </summary>
        /// <returns>An observable that ends when the effect has finished.</returns>
        public IObservable<Unit> HideEffect() => HideEffectCoroutine().ToObservable();

        /// <summary>
        /// Shows this transition effect on the screen, usually ending
        /// when the game-world is fully obscured.
        /// </summary>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        protected abstract IEnumerator ShowEffectCoroutine();

        /// <summary>
        /// Removes this transition effect from the screen, usually ending
        /// when the game-world is fully visible.
        /// </summary>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        protected abstract IEnumerator HideEffectCoroutine();
    }
}
