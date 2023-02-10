using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Vector3 _goalPosition;
    [SerializeField] private Vector3 _rotationGoal;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _goalScale = 2;
    private float _current, _target;
    // Start is called before the first frame update
    void Start()
    {
        var myValue = Mathf.Lerp(0, 10, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) _target = _target == 0? 1 : 0;

        _current = Mathf.MoveTowards(_current, _target, _speed * Time.deltaTime);

        transform.position = Vector3.Lerp(Vector3.zero, _goalPosition, _current.Evaluate(_current));
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(_rotationGoal), _curve.Evaluate(current));
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one *  goalScale, _curve.Evaluate(Mathf.PingPong(_current, 0.5f * 2)));

    }
}