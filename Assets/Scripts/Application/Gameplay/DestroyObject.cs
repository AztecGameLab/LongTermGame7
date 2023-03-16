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

    public class DestroyObject : MonoBehaviour
    {
        public void Interact()
        {
            Destroy(gameObject);
            Debug.Log(gameObject + " is dead.");
        }
    }
}