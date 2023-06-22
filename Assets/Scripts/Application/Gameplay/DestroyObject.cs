namespace Application.Gameplay
{
    /*
     * In here the object's popup window will be implemented.
     *
     */
    using System;
    using System.Threading.Tasks;
    using Core;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

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
