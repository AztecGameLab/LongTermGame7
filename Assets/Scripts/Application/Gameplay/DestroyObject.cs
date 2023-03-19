namespace Application.Gameplay
{
    /*
     * In here the object's popup window will be implemented.
     *
     */
    using UnityEngine;

    /// <summary>
    /// Destroys an object when commanded.
    /// </summary>
    public class DestroyObject : MonoBehaviour
    {
        /// <summary>
        /// Destroyed this object.
        /// </summary>
        public void Interact()
        {
            Destroy(gameObject);
            Debug.Log(gameObject + " is dead.");
        }
    }
}
