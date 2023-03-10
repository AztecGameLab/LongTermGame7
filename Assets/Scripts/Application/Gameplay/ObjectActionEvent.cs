using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectActionEvent : MonoBehaviour
{
    [SerializeField] public KeyCode interactKey;
    [SerializeField] public UnityEvent interactAction;

    /*
     * After we interact with an NPC/object 'interactable' canbe set to false so the player is not able to interact with them again.
     */
    public void InteractWith()
    {
        if (Input.GetKeyDown(interactKey))
        {
            interactAction.Invoke();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("Player in range of " + gameObject.tag);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("Player out of range " + gameObject.tag);
        }
    }
}

public interface IInteractable
{
    void Interact();
}