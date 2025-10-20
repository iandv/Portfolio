using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public AsteroidFactory factory;
    public float trajectory = 15f;
    public float spawnRate = 2f;
    public float spawnRadius = 15f;
    public int spawnAmount = 1;
    public ObjectPool<Asteroid> pool;
    private LookUpTable<GameObject, GameObject> _lookUpTable;
    private bool _rewind;

    public static AsteroidSpawner Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        _rewind = false;
        _lookUpTable = new LookUpTable<GameObject, GameObject>(GetPrefab);
        factory = new AsteroidFactory();
    }

    void Start()
    {
        var prefab = Resources.Load<GameObject>("Asteroid");
        _lookUpTable.GetValue(prefab);
        factory.prefab = _lookUpTable[prefab];
        float bounds = spawnRadius + 1;
        pool = new ObjectPool<Asteroid>(20, factory, bounds, Asteroid.TurnOn, Asteroid.TurnOff);
        OEventManager.Instance.Subscribe("OnPlayerDeath", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("OnVictory", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("Rewind", Rewind);
        OEventManager.Instance.Subscribe("StopRewind", StopRewind);
        Spawn();
    }

    void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
        if (!_rewind)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnRadius;
                Vector3 spawnPoint = transform.position + spawnDirection;

                float variation = Random.Range(-trajectory, trajectory);
                Quaternion rotation = Quaternion.AngleAxis(variation, Vector3.forward);
                Asteroid asteroid = pool.Get();
                asteroid.pool = pool;
                asteroid.transform.position = spawnPoint;
                asteroid.transform.rotation = rotation;
                asteroid.AsteroidStart();
                asteroid.SetTrajectory(rotation * -spawnDirection);
                ListOfAsteroids.Instance.Add(asteroid);
            }
        }        
    }

    public void LoadAsteroids(ListOfAsteroidsData data)
    {
        for (int i = 0; i < data.indexLenght; i++)
        {
            if (i < data.indexLenght)
            {
                Asteroid asteroid = pool.Get();
                asteroid.pool = pool;
                asteroid.transform.position = data.positions[i];
                asteroid.transform.rotation = data.rotations[i];
                asteroid.GetComponent<Rigidbody2D>().velocity = data.velocities[i];
                asteroid.chosenSprite = data.chosenSprites[i];
                asteroid.ChangeSprite(data.chosenSprites[i]);
                asteroid.transform.localScale = Vector3.one * data.sizes[i];
                ListOfAsteroids.Instance.Add(asteroid);
            }
        }
    }

    private GameObject GetPrefab(GameObject prefab)
    {
        return prefab;
    }

    private void Rewind(params object[] parameters)
    {
        _rewind = true;
    }

    private void StopRewind(params object[] parameters)
    {
        _rewind = false;
    }

    private void OnVictoryOrDeath(params object[] parameters)
    {
        gameObject.SetActive(false);
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnRate);
        Spawn();
    }
}
