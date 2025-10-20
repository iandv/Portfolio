using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfAsteroids : MonoBehaviour
{
    public string savePath = "ListOFAsteroids.dat";
    public List<SerializableVector3> positions;
    public List<SerializableQuaternion> rotations;
    public List<SerializableVector2> velocities;
    public List<int> chosenSprites;
    public List<float> sizes;
    public int indexLenght;
    public List<Asteroid> _subscribedAsteroids;

    public static ListOfAsteroids Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnSave", SaveData);
        OEventManager.Instance.Subscribe("OnLoad", LoadData);
    }
    public void Add(Asteroid asteroid)
    {
        _subscribedAsteroids.Add(asteroid);
    }

    public void Remove(Asteroid asteroid)
    {
        _subscribedAsteroids.Remove(asteroid);
    }

    void SaveInfo()
    {
        foreach(Asteroid asteroid in _subscribedAsteroids)
        {
            var aPosition = new SerializableVector3(asteroid.transform.position);
            positions.Add(aPosition);
            var aRotation = new SerializableQuaternion(asteroid.transform.rotation);
            rotations.Add(aRotation);
            var aVelocity = new SerializableVector2(asteroid.GetComponent<Rigidbody2D>().velocity);
            velocities.Add(aVelocity);
            chosenSprites.Add(asteroid.chosenSprite);
            sizes.Add(asteroid.size);
        }
        indexLenght = _subscribedAsteroids.Count;
    }

    void ClearAsteroid()
    {
        OEventManager.Instance.Trigger("ReturnAll");
    }

    void SaveData(params object[] parameters)
    {
        SaveInfo();
        var lOAsteroidsData = new ListOfAsteroidsData(this);
        lOAsteroidsData.SaveBinary(Application.dataPath + savePath);
    }

    IEnumerator ClearCoroutine()
    {
        yield return null;
        var lOAsteroidsData = BinarySerializer.LoadBinary<ListOfAsteroidsData>(Application.dataPath + savePath);
        positions = lOAsteroidsData.positions;
        rotations = lOAsteroidsData.rotations;
        velocities = lOAsteroidsData.velocities;
        chosenSprites = lOAsteroidsData.chosenSprites;
        sizes = lOAsteroidsData.sizes;
        indexLenght = lOAsteroidsData.indexLenght;
        AsteroidSpawner.Instance.LoadAsteroids(lOAsteroidsData);
    }

    void LoadData(params object[] parameters)
    {
        ClearAsteroid();
        StartCoroutine(ClearCoroutine());
    }
}
