using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IObservable
{
    public string savePath = "Player.dat";
    private List<IObserver> _subscribers = new List<IObserver>();
    private int _currentWeaponIndex;
    private Rigidbody2D _rigidBody;
    private bool _invulnerable;
    private bool _death;
    private bool _paused;
    private int _currentHealth;

    private PlayerModel _playerModel;
    private IWeapon _currentWeapon;
    private bool _activeRewind, _yAxis, _xAxis;
    private float _force, _forceRot, _dirY, _dirX;

    void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _invulnerable = false;
        _death = false;
        _paused = false;
    }

    void Start()
    {
        _currentHealth = _playerModel.health;
        _currentWeapon = _playerModel.weapons[_currentWeaponIndex];
        OEventManager.Instance.Subscribe("OnVictory", OnVictory);
        OEventManager.Instance.Subscribe("Rewind", Rewind);
        OEventManager.Instance.Subscribe("StopRewind", StopRewind);
        OEventManager.Instance.Subscribe("OnPause", OnPause);
        OEventManager.Instance.Subscribe("OnSave", SaveData);
        OEventManager.Instance.Subscribe("OnLoad", LoadData);
        StartCoroutine(StartHealthCoroutine());
    }

    void Update()
    {
        if (!_death)
        {
            CheckBounds();
        }
    }

    void FixedUpdate()
    {
        if (!_death)
        {
            ApplyForce();
            Movement();
        }
    }

    void SaveData(params object[] parameters)
    {
        var playerData = new PlayerData(this);
        playerData.currentHealth = _currentHealth;
        playerData.currentWeaponIndex = _currentWeaponIndex;
        playerData.SaveBinary(Application.dataPath + savePath);
    }

    void LoadData(params object[] parameters)
    {
        var playerData = BinarySerializer.LoadBinary<PlayerData>(Application.dataPath + savePath);
        _currentHealth = playerData.currentHealth;
        _currentWeaponIndex = playerData.currentWeaponIndex;
        _currentWeapon = _playerModel.weapons[_currentWeaponIndex];
        transform.position = playerData.position;
        transform.rotation = playerData.rotation;
        _rigidBody.velocity = new Vector2(0, 0);
        OEventManager.Instance.Trigger("OnPlayerDamage", _currentHealth);
    }

    void Movement()
    {
        transform.Rotate(0, 0, -_forceRot * _playerModel.rotationSpeed * Time.deltaTime);
        _rigidBody.AddForce(transform.up * _playerModel.thrust * _force);
        _rigidBody.velocity = new Vector2(Mathf.Clamp(_rigidBody.velocity.x, -_playerModel.maxSpeed, _playerModel.maxSpeed),
            Mathf.Clamp(_rigidBody.velocity.y, -_playerModel.maxSpeed, _playerModel.maxSpeed));
    }

    void ApplyForce()
    {
        if (_yAxis)
        {
            if (Mathf.Abs(_force) > 1f)
                _force = 1f * _dirY;

            else if (Mathf.Abs(_force) < 1f)
                _force += Time.deltaTime * _dirY;
        }

        if (!_yAxis)
            _force = 0f;

        if (_xAxis)
        {
            if (Mathf.Abs(_forceRot) > 1f)
                _forceRot = 1f * _dirX;

            else if (Mathf.Abs(_forceRot) < 1f)
                _forceRot += Time.deltaTime * _dirX;
        }

        if (!_xAxis)
            _forceRot = 0f;
    }

    public void InputAxisRotate(bool press, int direction)
    {
        if (_death) return;

        _xAxis = press;
        _dirX = direction;
    }

    public void InputAxisForce(bool press, int direction)
    {
        if (_death) return;

        _yAxis = press;
        _dirY = direction;
    }

    //float CalculateForce(float force, int direction)
    //{
    //    if (force > 1f)
    //        force = 1f;

    //    if (force < 1f)
    //        force += Time.deltaTime;

    //    return force *= direction;
    //}

    void CheckBounds()
    {
        if (transform.position.y > _playerModel.screenBoundariesY) transform.position = new Vector3(transform.position.x, -_playerModel.screenBoundariesY, transform.position.z);
        if (transform.position.y < -_playerModel.screenBoundariesY) transform.position = new Vector3(transform.position.x, _playerModel.screenBoundariesY, transform.position.z);
        if (transform.position.x > _playerModel.screenBoundariesX) transform.position = new Vector3(-_playerModel.screenBoundariesX, transform.position.y, transform.position.z);
        if (transform.position.x < -_playerModel.screenBoundariesX) transform.position = new Vector3(_playerModel.screenBoundariesX, transform.position.y, transform.position.z);
    }

    public void Shoot()
    {
        if (!_death && !_paused)
        {
            _currentWeapon.Shoot();
        }
    }

    public void ShootTwo()
    {
        if (!_death && !_paused)
        {
            _currentWeapon.ShootTwo();
        }
    }

    public void OnPowerUpPickUp(IWeapon powerUp)
    {
        OEventManager.Instance.Trigger("OnPickUp");
        powerUp.Next = _currentWeapon;
        _currentWeapon = powerUp;
    }

    public void ReturnWeapon()
    {
        _currentWeapon = _currentWeapon.Next;
    }

    public void Healing()
    {
        if (_currentHealth < _playerModel.health)
        {
            _currentHealth++;
            OEventManager.Instance.Trigger("OnPlayerDamage", _currentHealth);
            _currentWeapon = _currentWeapon.Next;
        }
    }

    public void StopLaser()
    {
        if (!_death && !_paused)
        {
            LaserGun laser = _currentWeapon.GetComponent<LaserGun>();
            if (laser != null)
            {
                laser.StopLaser();
            }
        }        
    }

    public void WeaponSwitch()
    {
        _currentWeaponIndex++;
        if (_currentWeaponIndex >= _playerModel.weapons.Count) _currentWeaponIndex = 0;
        _currentWeapon = _playerModel.weapons[_currentWeaponIndex];
    }

    IEnumerator InvulnerableCoroutine()
    {
        _invulnerable = !_invulnerable;
        Notify("OnStartDamageEffect");
        yield return new WaitForSeconds(_playerModel.invulnerableTime);
        _invulnerable = !_invulnerable;
        Notify("OnEndDamageEffect");
    }

    IEnumerator StartHealthCoroutine()
    {
        yield return null;
        OEventManager.Instance.Trigger("OnPlayerDamage", _currentHealth);
    }

    void OnPlayerDeath()
    {
        OEventManager.Instance.Trigger("OnPlayerDeath");
        _death = true;
        Notify("OnDeathEffect");
    }

    public void OnPause(params object[] parameters)
    {
        _paused = !_paused;
    }

    void OnVictory(params object[] parameters)
    {
        _death = true;
    }

    void Rewind(params object[] parameters)
    {
        _activeRewind = true;
    }

    void StopRewind(params object[] parameters)
    {
        _activeRewind = false;
    }

    public void Subscribe(IObserver observer)
    {
        _subscribers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        _subscribers.Remove(observer);
    }

    public void Notify(string eventID)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.OnNotify(eventID);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() && !_invulnerable)
        {
            _currentHealth--;
            OEventManager.Instance.Trigger("OnPlayerDamage", _currentHealth);
            if (_currentHealth > 0)
                StartCoroutine(InvulnerableCoroutine());
            else
                OnPlayerDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.gameObject.GetComponent<HealthPackDecorator>();
        if (health != null && _currentHealth < _playerModel.health)
        {
            health.ActivePowerUp(this);
            Shoot();
        }

        var rewind = collision.gameObject.GetComponent<RewindPowerUpDecorator>();
        if (rewind != null && !_activeRewind)
        {
            rewind.ActivePowerUp(this);
            Shoot();
        }
    }
}