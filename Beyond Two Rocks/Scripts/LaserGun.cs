using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LaserGun : IWeapon
{
    public float range;
    public float batteryCharge;
    public Transform laserSpawnPoint;
    public LineRenderer lineRenderer;
    private LayerMask _layerMask;

    private AudioSource _audioSource;
    float _currentCharge = 0f;
    private bool _isFiring;

    private void Awake()
    {
        _isFiring = false;
        _layerMask = LayerMask.GetMask("Default");
        _audioSource = GetComponent<AudioSource>();
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, range, 0));
        lineRenderer.enabled = false;
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnPlayerDeath", OnVictoryOrDeath);
        OEventManager.Instance.Subscribe("OnVictory", OnVictoryOrDeath);
    }

    public void Update()
    {
        if (!_isFiring && _currentCharge > 0)
        {
            _currentCharge -= Time.deltaTime / 2;
            OEventManager.Instance.Trigger("OnBatteryCharge", _currentCharge, batteryCharge);
        }
    }

    private void OnVictoryOrDeath(params object[] parameters)
    {
        gameObject.SetActive(false);
    }

    public override void Shoot()
    {
        if (_currentCharge >= batteryCharge)
            lineRenderer.enabled = false;

        _isFiring = true;

        if (_currentCharge <= batteryCharge)
        {
            _currentCharge += Time.deltaTime;
            OEventManager.Instance.Trigger("OnBatteryCharge", _currentCharge, batteryCharge);
            _audioSource.Play();
            lineRenderer.enabled = true;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, range, _layerMask);
            if (hit)
            {
                Asteroid asteroid = hit.transform.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.DestroyAsteroid();
                }
            }
        }       
    }

    public void StopLaser()
    {
        _isFiring = false;
        lineRenderer.enabled = false;
    }
}
