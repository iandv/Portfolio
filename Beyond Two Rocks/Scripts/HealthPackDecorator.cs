using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackDecorator : IWeapon
{
    private PlayerController _player;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider;
    private bool _onPickUp;

    public new IWeapon Next
    {
        get; set;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnPickUp", OnPickUpPowerUp);
    }

    public override void Shoot()
    {
        Healing();
    }

    void Healing()
    {
        _player.Healing();
        OEventManager.Instance.Trigger("OnPickUp");
        Destroy(gameObject);
    }

    void OnPickUpPowerUp(params object[] parameters)
    {
        _onPickUp =! _onPickUp;
    }

    public void ActivePowerUp(PlayerController controller)
    {
        if (!_onPickUp)
        {
            _player = controller;
            controller.OnPowerUpPickUp(this);
        }       
    }
}