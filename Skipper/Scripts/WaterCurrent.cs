using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour
{
    [SerializeField]
    float force;
    [SerializeField]
    Vector3 direction;

    private void OnTriggerStay(Collider other)
    {
        var player = other.gameObject.GetComponentInParent<ShipBehaviour>();
        if (player != null)
        {
            player.Impulse(force, direction);
        }
    }
}
