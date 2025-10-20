using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubmarineView : MonoBehaviour, IObservable
{
    PlayerSubmarineModel _subModel;
    AudioSource _au;
    bool _isAlreadyInSurface;
    List<IObserver> _subscribers = new List<IObserver>();


    private void Awake()
    {
        _subModel = GetComponent<PlayerSubmarineModel>();
        _au = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBackgroundSounds();
    }

    private void Update()
    {
        if (_isAlreadyInSurface != _subModel.onSurface)
        {
            _isAlreadyInSurface = _subModel.onSurface;
            PlayBackgroundSounds();
        }
        PlayMovementdSounds();
    }


    void PlayBackgroundSounds()
    {
        if (_subModel.onSurface)
        {
            Notify("OnSurface");
        }

        else
        {
            Notify("UnderWater");
        }
    }

    void PlayMovementdSounds()
    {
        if (Mathf.Abs(_subModel.rb.velocity.z) > 0.1f)
        {
            _au.clip = _subModel.movementSounds;
            _au.Play();
        }

        else if (Mathf.Abs(_subModel.rb.velocity.z) < 0.1f)
        {
            _au.clip = null;
            _au.Stop();
        }
    }

    public void StartLitUpCoroutine(float time)
    {
        StartCoroutine(EffectCoroutine(time));
    }

    IEnumerator EffectCoroutine(float time)
    {
        Notify("LitUpPart");
        yield return new WaitForSeconds(time);
        Notify("TurnOffPart");
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

}
