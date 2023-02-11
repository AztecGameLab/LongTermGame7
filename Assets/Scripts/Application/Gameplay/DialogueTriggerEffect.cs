using Application.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

namespace Application.Gameplay
{
    [RequireComponent(typeof(Trigger))]
    public class DialogueTriggerEffect : MonoBehaviour
    {
        [SerializeField] private string nodeId;
        
        private Trigger _trigger;
        private DialogueRunner _dialogueRunner;

        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
            _dialogueRunner = FindObjectOfType<DialogueRunner>();
        }

        private void OnEnable()
        {
            _trigger.CollisionEnter += HandleCollisionEnter;
        }

        private void OnDisable()
        {
            _trigger.CollisionEnter -= HandleCollisionEnter;
        }
        
        private void HandleCollisionEnter(GameObject obj)
        {
            _dialogueRunner.StartDialogue(nodeId);
            _dialogueRunner.onDialogueComplete.AddListener(HandleDialogueComplete);

            FindObjectOfType<PlayerInput>().enabled = false;
        }

        private void HandleDialogueComplete()
        {
            FindObjectOfType<PlayerInput>().enabled = true;
        }
    }
}