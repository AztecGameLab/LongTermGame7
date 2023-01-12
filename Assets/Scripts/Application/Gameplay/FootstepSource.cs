using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] private float stepDistance = 1;
    
    private PhysicsComponent _physics;
    private float _elapsedDistance;                         //distance from one frame  of a step to another
    private Vector3 frameBefore_elapsedDistanced = 0,0,0;   //initial point to measure distance

    private void Start()
    {
        _physics = GetComponent<PhysicsComponent>();
    }

    private void Update()
    {
        if (_physics.IsGrounded)
        {
            _elapsedDistance = Vector3.Distance(transform.position, frameBefore_elapsedDistanced);
            // Update elapsedDistance with how far we have moved this frame.

            if (_elapsedDistance => stepDistance)  
            {  // Compare elapsedDistance to our step distance to see if we walked far enough

            Services.EventBus.Invoke(new stepEvent(), "Demo Step");
            // If we did, fire the event and reset elapsedDistance to 0
            _elapsedDistance = 0;
            }
        }
    }
    private void stepEvent()
    {
        Debug.log("Footstep sound!");      //Plug in footstep sound
    }
}

