using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindPowerUpDecorator : IWeapon
{
    public float effectTime;

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
        RewindEffect();
    }

    void RewindEffect()
    {
        OEventManager.Instance.Trigger("OnPickUp");
        OEventManager.Instance.Trigger("Rewind");
        _player.ReturnWeapon();
        StartCoroutine(EffectCoroutine());
    }

    void OnPickUpPowerUp(params object[] parameters)
    {
        _onPickUp = !_onPickUp;
    }

    public void ActivePowerUp(PlayerController controller)
    {
        if (!_onPickUp)
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;
            _player = controller;
            controller.OnPowerUpPickUp(this);
        }
    }

    private IEnumerator EffectCoroutine()
    {
        yield return new WaitForSeconds(effectTime);
        OEventManager.Instance.Trigger("StopRewind");
        Destroy(gameObject);
    }
}
