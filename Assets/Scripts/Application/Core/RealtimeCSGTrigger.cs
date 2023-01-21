using poetools;
using System;
using UnityEngine;

namespace Application.Core
{
    public class RealtimeCSGTrigger : MonoBehaviour
    {
        private void Start()
        {
            var col = GetComponentInChildren<Collider>();
            var trigger = col.gameObject.AddComponent<Trigger>();
            trigger.CollisionEnter.AddListener(_ => Debug.Log("Hi!"));
        }
    }
}