using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    protected Rigidbody _rb;
    protected int _currentHp;
    protected bool _dead;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //Force that comes from water currents
    public void Impulse(float speed, Vector3 direction)
    {
        _rb.AddForce(direction * speed);
    }

    public virtual void ReceiveDamage(int damage)
    {

    }

    protected virtual void Death()
    {

    }
}
