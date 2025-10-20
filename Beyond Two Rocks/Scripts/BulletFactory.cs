using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BulletFactory : IFactory<Bullet,float>
{
    public GameObject prefab;
    public Bullet Create(float value)
    {
        var obj = GameObject.Instantiate(prefab).GetComponent<Bullet>();

        return obj;
    }
}
