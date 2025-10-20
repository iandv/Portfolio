using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public ObjectPool<Asteroid> pool;
    public float screenBoundariesX;
    public float screenBoundariesY;
    public Sprite[] spriteList;
    public int chosenSprite;
    public float size = 1f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnPlayerDeath", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("OnVictory", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("ReturnAll", Return);
    }

    private void Update()
    {
        CheckBounds();
    }

    void CheckBounds()
    {
        if (transform.position.y > screenBoundariesY) Return();
        if (transform.position.y < -screenBoundariesY) Return();
        if (transform.position.x > screenBoundariesX) Return();
        if (transform.position.x < -screenBoundariesX) Return();
    }

    public void AsteroidStart()
    {
        size = Random.Range(AsteroidFlyweightPointer.config.minSize, AsteroidFlyweightPointer.config.maxSize);
        chosenSprite = Random.Range(0, spriteList.Length);
        ChangeSprite(chosenSprite);
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        _rigidBody.mass = size;
    }

    public void ChangeSprite(int index)
    {
        _spriteRenderer.sprite = spriteList[index];
    }

    private void OnVictoryOrDeath(params object[] parameters)
    {
        gameObject.SetActive(false);
    }

    public void SetTrajectory(Vector3 direction)
    {
        _rigidBody.AddForce(direction * AsteroidFlyweightPointer.config.speed);
    }

    public static Asteroid TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        return asteroid;
    }

    public static Asteroid TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        return asteroid;
    }

    public void Return(params object[] parameters)
    {
        ListOfAsteroids.Instance.Remove(this);
        pool.Return(this);
    }

    public void DestroyAsteroid()
    {
        OEventManager.Instance.Trigger("OnAsteroidDestruction", this);
        Return();
    }
}
