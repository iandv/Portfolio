using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public static implicit operator Vector2(SerializableVector2 vector)
    {
        return new Vector3(vector.x, vector.y);
    }
}
