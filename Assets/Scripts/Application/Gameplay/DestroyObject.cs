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

    public class DestroyObject : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Destroy(this);
            Debug.Log(gameObject + " is dead.");
        }
    }
}
