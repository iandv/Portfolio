using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControll : MonoBehaviour
{
    Transform _Player;
    float dist;
    public float atackDistance;
    public Transform cannon;
    public Transform support;
    public GameObject _harpoon;
    public float fireRate, nextFire, projectileSpeed;
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        dist = Vector3.Distance(_Player.position, transform.position);

        if (dist <= atackDistance)
        {
            cannon.LookAt(_Player);
            support.LookAt(_Player);
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject clone = Instantiate(_harpoon, cannon.position, cannon.rotation);
        clone.GetComponent<Rigidbody>().AddForce(clone.transform.forward * projectileSpeed);
        Destroy(clone, 10);
    }
}
