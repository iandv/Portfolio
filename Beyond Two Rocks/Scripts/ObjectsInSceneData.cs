using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectsInSceneData
{
    public List<SerializableVector3> listOfPositions;
    public List<SerializableQuaternion> listOfRotations;

    public ObjectsInSceneData(ListOfObjectsInScene list)
    {
        listOfPositions = new List<SerializableVector3>(list.positions);
        listOfRotations = new List<SerializableQuaternion>(list.rotations);
    }
}
