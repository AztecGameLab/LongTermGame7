using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMarkAnimation : MonoBehaviour
{
    [SerializeField] private float amplatude;
    [SerializeField] private float dialation;
    
    private Vector3 _defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        _defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _defaultPos + new Vector3(0,amplatude * Mathf.Sin(Time.time/dialation),0);
    }
}
