using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListOfAsteroidsData
{
    public List<SerializableVector3> positions;
    public List<SerializableQuaternion> rotations;
    public List<int> chosenSprites;
    public List<float> sizes;
    public List<SerializableVector2> velocities;
    public int indexLenght;

    public ListOfAsteroidsData(ListOfAsteroids lOfAsteroids)
    {
        positions = lOfAsteroids.positions;
        rotations = lOfAsteroids.rotations;
        chosenSprites = lOfAsteroids.chosenSprites;
        sizes = lOfAsteroids.sizes;
        velocities = lOfAsteroids.velocities;
        indexLenght = lOfAsteroids.indexLenght;
    }
}
