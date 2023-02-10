namespace Application.Vfx
{
    using System.Collections;

    /// <summary>
    /// A transition that can be used to hide the contents of the screen.
    /// </summary>
    public abstract class SceneTransition
    {
        /// <summary>
        /// Shows this transition effect on the screen, usually ending
        /// when the game-world is fully obscured.
        /// </summary>
        /// <returns>An IEnumerator for use in a Coroutine.</returns>
        public abstract IEnumerator ShowEffect();

        /// <summary>
        /// Removes this transition effect from the screen, usually ending
        /// when the game-world is fully visible.
        /// </summary>
        /// <returns>An IEnumerator for use in a Coroutine.</returns>
        public abstract IEnumerator HideEffect();
    }
}
