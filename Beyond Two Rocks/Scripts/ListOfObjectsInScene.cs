using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfObjectsInScene : MonoBehaviour
{
    public static ListOfObjectsInScene instance;

    public List<SerializableVector3> positions;
    public List<SerializableQuaternion> rotations;
    public int index;

    private void Awake()
    {
        instance = this;
    }

    public void Add(GameObject gameObject)
    {
        var sPosition = new SerializableVector3(gameObject.transform.position);
        var sRotation = new SerializableQuaternion(gameObject.transform.rotation);
        positions.Add(sPosition);
        rotations.Add(sRotation);
        index++;
    }

    public void Remove(GameObject gameObject)
    {
        var sPosition = new SerializableVector3(gameObject.transform.position);
        var sRotation = new SerializableQuaternion(gameObject.transform.rotation);
        positions.Remove(sPosition);
        rotations.Remove(sRotation);
        index--;
    }
}
