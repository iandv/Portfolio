using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public static implicit operator Vector3(SerializableVector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }
}
