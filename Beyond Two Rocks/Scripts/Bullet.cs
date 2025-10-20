using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ObjectPool<Bullet> pool;
    private Rigidbody2D _rigidBody;
    public float addedSpeed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnPlayerDeath", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("OnVictory", OnVictoryOrDeath);
    }

    private void Update()
    {
        _rigidBody.velocity = transform.up * BulletFlyweightPointer.config.speed;
    }

    private void OnVictoryOrDeath(params object[] parameters)
    {
        gameObject.SetActive(false);
    }

    public static Bullet TurnOn(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.StartTimeLife();
        return bullet;
    }

    public static Bullet TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    public IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(BulletFlyweightPointer.config.lifeTime);
        Return();
    }

    private void StartTimeLife()
    {
        StartCoroutine(LifeTimeCoroutine());
    }

    private void Return()
    {
        pool.Return(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.DestroyAsteroid();
            Return();
        }
    }
}
