using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private void Start()
    {
        var player = FindObjectOfType<Player>();
        transform.parent = player.transform;
    }
}
