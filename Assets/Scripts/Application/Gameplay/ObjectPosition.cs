using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPosition : MonoBehaviour
{
    public GameObject lastHit;
    public Vector3 collision = Vector3.zero;
    private RaycastHit hit;


    // Update is called once per frame
    void Update()
    {
        GetObjectPosition();
    }

    void GetObjectPosition()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(collision, 5.2f);
        Debug.DrawRay(transform.position, collision);
    }

}
