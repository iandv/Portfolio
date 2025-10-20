using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSubmarineController : ShipBehaviour
{
    PlayerSubmarineView _subView;
    PlayerSubmarineModel _subModel;
    float _yDir, _batteryCharge;
    bool _energyDepleted, _goingUp;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
        _subModel = GetComponent<PlayerSubmarineModel>();
        _subView = GetComponent<PlayerSubmarineView>();
        _energyDepleted = false;
    }

    private void Start()
    {
        _subModel.onSurface = OnSurface();
        _subModel.rb = GetComponent<Rigidbody>();
        _batteryCharge = _subModel.maxBatteryCharge;
        _currentHp = _subModel.maxHealth;
        _subModel.UpWarning.SetActive(false);
        _subModel.lifeCounter.text = "x" + _currentHp.ToString();
    }

    private void Update()
    {
        if (!_dead)
        {
            Energy();
            PauseGame();
        }

        if (_dead)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        if (!_dead)
        {

            if (!_energyDepleted)
            {
                if (_subModel.onSurface)
                {
                    Movement(_subModel.maxSurfaceSpeed);
                    _rb.velocity = new Vector3(_rb.velocity.x, 1, _rb.velocity.z);
                }

                else
                    Movement(_subModel.maxUnderWaterSpeed);
            }

            else
                GoBackToSurface();
        }
    }

    public override void ReceiveDamage(int damage)
    {
        if (!_dead)
        {
            _currentHp--;
            _subModel.lifeCounter.text = "x" + _currentHp.ToString();
            _subView.StartLitUpCoroutine(_subModel.damageFeedback);
            if (_currentHp <= 0)
            {
                Death();
            }
        }
    }

    float YDirection()
    {
        bool up = false, down = false;

        if (Input.GetKey(KeyCode.Space))
        {
            _subModel.onSurface = OnSurface();

            if (!_subModel.onSurface && !down)
            {
                _yDir += Time.deltaTime;
                up = true;
            }
        }

        if (!_goingUp)
        {
            if (Input.GetKey(KeyCode.LeftControl) && !(Input.GetKey(KeyCode.Space)))
            {
                if (_subModel.onSurface)
                    _subModel.onSurface = false;

                _yDir -= Time.deltaTime;
                down = true;

                _rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
        }

        if (!up && !down)
        {
            //return _yDir < -0.1 ? _yDir += Time.deltaTime : _yDir > 0.1 ? _yDir -= Time.deltaTime : _yDir = 0;
            _yDir = 0;
        }

        if (_rb.velocity.y > 0 && _subModel.onSurface)
        {
            _yDir = 0;

            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

        return _yDir = Mathf.Clamp(_yDir, -1, 1);
    }

    void Energy()
    {
        if (_subModel.onSurface)
        {
            _subModel.UpWarning.SetActive(false);
            _energyDepleted = false;
        }

        if (!_subModel.onSurface && _batteryCharge > 0)
        {
            _batteryCharge -= Time.deltaTime;
        }

        if(_subModel.onSurface && _batteryCharge < _subModel.maxBatteryCharge)
        {
            _batteryCharge += Time.deltaTime * _subModel.rechargeModifier;
        }

        else if (_batteryCharge <= 0)
        {
            _subModel.UpWarning.SetActive(true);
            _energyDepleted = true;
        }

        _subModel.batteryMeter.fillAmount = _batteryCharge / _subModel.maxBatteryCharge;
    }

    void GoBackToSurface()
    {
        _subModel.onSurface = OnSurface();
        _rb.AddRelativeForce(Vector3.up * _subModel.noEnergySpeed);
        if (!_subModel.onSurface)
            _goingUp = true;
        else if (_subModel.onSurface)
            StartCoroutine(GoingUpCoroutine());
    }

    IEnumerator GoingUpCoroutine()
    {
        yield return new WaitForSeconds(1);
        _goingUp = false;
    }

    bool OnSurface()
    {
        return Physics.CheckSphere(_subModel.surfaceCheck.position, _subModel.surfaceDistance, _subModel.layerMask);
    }

    void Movement(float speed)
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * _subModel.rotationSpeed * Time.deltaTime, 0);

        _rb.AddRelativeForce((Vector3.up * YDirection() + (Vector3.forward * Input.GetAxis("Vertical")).normalized) * _subModel.thrust);
        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -speed, speed), Mathf.Clamp(_rb.velocity.y, -speed, speed), Mathf.Clamp(_rb.velocity.z, -speed, speed));
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CanvasPause.Instance.PauseGame();
    }

    protected override void Death()
    {
        _dead = true;
        Cursor.lockState = CursorLockMode.None;
        _subModel.deathScreen.SetActive(true);
    }
}
