using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AsteroidFlyweightPointer
{
    public static AsteroidFlyweight config = new AsteroidFlyweight()
    {
        score = 100,
        speed = 5,
        minSize = 0.5f,
        maxSize = 1.5f
    };
}
