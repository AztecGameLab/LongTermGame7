using UnityEngine;
using UnityEngine.Events;

public class ObjectActionEvent : MonoBehaviour
{
    [SerializeField] private bool interactable;
    [SerializeField] public KeyCode interactKey;
    [SerializeField] public UnityEvent interactAction;

    /*
     * After we interact with an NPC/object 'interactable' canbe set to false so the player is not able to interact with them again.
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
