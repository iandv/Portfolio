using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyweightPointer
{
    public static BulletFlyweight config = new BulletFlyweight()
    {
        speed = 10f,
        lifeTime = 3f
    };
}
