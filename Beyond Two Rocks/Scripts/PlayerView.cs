using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, IObserver
{
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private PlayerModel _playerModel;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _playerModel = GetComponent<PlayerModel>();
        var playerController = GetComponent<PlayerController>();
        playerController.Subscribe(this);
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("OnVictory", OnVictory);
    }

    public void OnStartDamageEffect()
    {
        _audioSource.PlayOneShot(_playerModel.damageAudioClip);
        _spriteRenderer.color = Color.red;
    }

    public void OnEndDamageEffect()
    {
        _spriteRenderer.color = Color.white;
    }

    public void OnDeathEffect()
    {
        _audioSource.PlayOneShot(_playerModel.deathAudioClip);
        _spriteRenderer.enabled = false;
    }

    public void OnVictory(params object[] parameters)
    {
        _audioSource.PlayOneShot(_playerModel.winAudioClip);
    }

    public void OnNotify(string eventID)
    {
        if (eventID == "OnStartDamageEffect") OnStartDamageEffect();
        if (eventID == "OnEndDamageEffect") OnEndDamageEffect();
        if (eventID == "OnDeathEffect") OnDeathEffect();
    }
}
