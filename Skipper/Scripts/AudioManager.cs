using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IObserver
{
    [SerializeField]
    AudioClip surfaceSound, underwaterSound;

    AudioSource _au;

    void Awake()
    {
        _au = GetComponent<AudioSource>();
    }

    void Start()
    {
        var toSubscribe = FindObjectOfType<PlayerSubmarineView>();
        toSubscribe.Subscribe(this);
    }

    public void OnNotify(string eventID)
    {
        if (eventID == "OnSurface") SurfaceAmbientSound(true);

        if (eventID == "UnderWater") SurfaceAmbientSound(false);
    }

    void SurfaceAmbientSound(bool surface)
    {
        if (surface)
        {
            _au.clip = surfaceSound;
            _au.Play();
        }

        else
        {
            _au.clip = underwaterSound;
            _au.Play();
        }
    }
}
