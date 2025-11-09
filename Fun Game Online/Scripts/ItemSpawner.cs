using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemSpawner : MonoBehaviourPun
{
    public static ItemSpawner instance;
    [SerializeField]
    public GameObject item;
    [SerializeField]
    public Vector3 center, size;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private int numberOfTimesItSpawns, spawnAmountOfItems;

    private int _numOfTimesItSpawned;

    private void Awake()
    {
        instance = this;
    }

    public void StartSpawner()
    {
        StartCoroutine(SpawnCoroutine());
    }
    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnRate);
        for (int i = 0; i < spawnAmountOfItems; i++)
        {
            Vector3 position = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
            PhotonNetwork.Instantiate(item.name, position, Quaternion.identity);
        }
        _numOfTimesItSpawned++;
        if (_numOfTimesItSpawned <= numberOfTimesItSpawns)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }
}
