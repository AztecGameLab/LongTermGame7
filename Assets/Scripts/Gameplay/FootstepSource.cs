using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] private float stepDistance = 1;
    
    private PhysicsComponent _physics;
    private float _elapsedDistance;

    private void Start()
    {
        _physics = GetComponent<PhysicsComponent>();
    }

    private void Update()
    {
        // if (_physics.IsGrounded)
        // {
        //     // Update elapsedDistance with how far we have moved this frame.

        //     if (_elapsedDistance => stepDistance)  
        //     {  // Compare elapsedDistance to our step distance to see if we walked far enough

        //     Services.EventBus.Invoke( data new stepEvent(), debugID "Demo Step");
        //     // If we did, fire the event and reset elapsedDistance to 0
        //     _elapsedDistance = 0;
        //     }
        // }
    }
    private void OnGUI()
    {
        if(GUILayout.Button(text "Step")){
        Services.EventBus.Invoke( data new stepEvent(), debugID "Demo Step");
        }
    }
    private void stepEvent()
    {
        private GameObject object;
    }
}

