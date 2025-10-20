using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    public GameObject[] prefabs;
    [Range(0, 100)]
    public float spawnChance;

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnAsteroidDestruction", OnAsteroidDestruction);
    }

    public void OnAsteroidDestruction(params object[] parameters)
    {
        Asteroid asteroid = (Asteroid)parameters[0];
        float roll = CalculateChance();
        if (roll <= spawnChance)
        {
            int index = ChoosePowerUp();
            var item = Instantiate(prefabs[index]);
            item.transform.position = asteroid.transform.position;
        }
    }

    private float CalculateChance()
    {
        float result = Random.Range(0.1f, 100);
        return result;
    }

    private int ChoosePowerUp()
    {
        int result = Random.Range(0, prefabs.Length);
        return result;
    }
}
