using System;
using Application.Core;
using UnityEngine;

public struct StepEvent
{

}

public class FootstepSource : MonoBehaviour
{
    [SerializeField] private float stepDistance = 1;
    
    private PhysicsComponent _physics;
    private float _elapsedDistance;                         //distance from one frame  of a step to another
    private Vector3 frameBefore_elapsedDistanced = new Vector3(0,0,0);   //initial point to measure distance

    private IDisposable _disposable;

    private void Start()
    {
        _physics = GetComponent<PhysicsComponent>();
        _disposable = Services.EventBus.AddListener<StepEvent>(stepEvent, "Testing Step Listener");
    }

    private void OnDestroy()
    {
        _disposable.Dispose();
    }

    private void Update()
    {
        // if (_physics.IsGrounded)
        // {
            _elapsedDistance += Vector3.Distance(transform.position, frameBefore_elapsedDistanced);
            // Debug.DrawLine(Vector3.zero, transform.position, Color.red, 0);
            // Update elapsedDistance with how far we have moved this frame.

            if (_elapsedDistance >= stepDistance)  
            {  // Compare elapsedDistance to our step distance to see if we walked far enough

                Services.EventBus.Invoke(new StepEvent(), "Demo Step");
                // If we did, fire the event and reset elapsedDistance to 0
                _elapsedDistance = 0;
            }

            frameBefore_elapsedDistanced = transform.position;
        // }
    }

    // private void OnGUI()
    // {
    //    GUILayout.Label($"elapsedDistance: {_elapsedDistance}");
    //    GUILayout.Label($"isGrounded: {_physics.IsGrounded}");
    //    GUILayout.Label($"isGrounded: {Vector3.Distance(transform.position, frameBefore_elapsedDistanced)}");
    // }

    private void stepEvent(StepEvent stepEvent)
    {
        Debug.Log("Footstep sound!");      //Plug in footstep sound
    }
}