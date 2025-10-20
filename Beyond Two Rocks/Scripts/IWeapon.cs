using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour
{
    public IWeapon Next
    {
        get; set;
    }

    public virtual void Shoot()
    {

    }

    public virtual void ShootTwo()
    {

    }
}