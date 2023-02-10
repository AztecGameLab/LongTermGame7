namespace Application.Core
{
    using UnityEngine;

    /// <summary>
    /// Provides utility methods for polling the Unity input API.
    /// </summary>
    public static class InputTools
    {
        /// <summary>
        /// Polls the alpha-numeric keys to see if a number has just been pressed.
        /// </summary>
        /// <param name="number">The number that's being pressed, if any. Within the range [1, 9].</param>
        /// <returns>Whether an alpha-numeric key is being pressed.</returns>
        public static bool TryGetNumberDown(out int number)
        {
            const int maxNumber = 9;
            const int offsetToKeyCode1 = 48;

            for (int i = 1; i <= maxNumber; i++)
            {
                if (Input.GetKeyDown((KeyCode)i + offsetToKeyCode1))
                {
                    number = i;
                    return true;
                }
            }

            number = -1;
            return false;
        }
    }
}
