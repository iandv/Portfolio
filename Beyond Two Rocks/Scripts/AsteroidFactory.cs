using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AsteroidFactory : IFactory<Asteroid, float>
{
    public GameObject prefab;

    public Asteroid Create(float value)
    {
        var obj = GameObject.Instantiate(prefab).GetComponent<Asteroid>();
        obj.screenBoundariesX = (int)value;
        obj.screenBoundariesY = (int)value;

        return obj;
    }
}
