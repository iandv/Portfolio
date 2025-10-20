using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineBehaviour : MonoBehaviour
{
    [SerializeField]
    protected int damage;

    private void OnCollisionEnter(Collision collision)
    {
        var ship = collision.gameObject.GetComponent<ShipBehaviour>();
        if (ship != null)
        {
            ship.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }
}
