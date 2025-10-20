using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRotation : MonoBehaviourPun
{
    private Camera _cam;

    private void Awake()
    {
        _cam = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        MouseAim();
    }

    private void MouseAim()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
