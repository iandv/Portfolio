using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGun : IWeapon
{
    public float fireRate;
    public Transform bulletSpawnPoint;
    private BulletFactory _factory;
    private ObjectPool<Bullet> _pool;
    private AudioSource _audioSource;
    private float _timeToFire = 0f;
    private LookUpTable<GameObject, GameObject> _lookUpTable;

    private void Awake()
    {
        _factory = new BulletFactory();
        _lookUpTable = new LookUpTable<GameObject, GameObject>(GetPrefab);
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var prefab = Resources.Load<GameObject>("Bullet");
        _lookUpTable.GetValue(prefab);
        _factory.prefab = _lookUpTable[prefab];
        _pool = new ObjectPool<Bullet>(20, _factory, 0, Bullet.TurnOn, Bullet.TurnOff);
    }

    public override void Shoot()
    {
        if (Time.time >= _timeToFire)
        {
            _timeToFire = Time.time + 1f / fireRate;
            _audioSource.Play();
            var bullet = _pool.Get();
            bullet.pool = _pool;
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.up = transform.up;
        }
        
    }

    private GameObject GetPrefab(GameObject prefab)
    {
        return prefab;
    }
}
