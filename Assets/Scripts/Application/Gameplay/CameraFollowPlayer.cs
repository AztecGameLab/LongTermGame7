using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform FollowTarget;
    private CinemachineVirtualCamera v_cam;

    // Start is called before the first frame update
    void Start()
    {
        v_cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        FollowTarget = Player.transform;
        v_cam.LookAt = FollowTarget;
        v_cam.Follow = FollowTarget;
    }
}
