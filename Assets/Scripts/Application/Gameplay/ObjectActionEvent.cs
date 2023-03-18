namespace Application.Gameplay
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// An object that can perform an action.
    /// </summary>
    public class ObjectActionEvent : MonoBehaviour
    {
        [SerializeField]
        private bool interactable;

        [SerializeField]
        private KeyCode interactKey;

        [SerializeField]
        private UnityEvent interactAction;

        /*
     * After we interact with an NPC/object 'interactable' can be set to false so the player is not able to interact with them again.
     */
        private void Update()
        {
            if (Input.GetKeyDown(interactKey) && interactable)
            {
                interactAction.Invoke();
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                interactable = true;
                Debug.Log("Player in range of " + gameObject.tag);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                interactable = false;
                Debug.Log("Player out of range " + gameObject.tag);
            }
        }
    }
}
