using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMemento : MonoBehaviour, IReminder
{
    public float maxRewindTime;
    private Memento<AsteroidSnapshot> _memento = new Memento<AsteroidSnapshot>();
    private Rigidbody2D _rigidBody;

    private Vector3 _velocity;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MementoManager.instance.Add(this);
    }

    public void MakeSnapshot()
    {
        if (_memento.snapshots.Count > Mathf.Round(maxRewindTime / Time.fixedDeltaTime))
        {
            _memento.snapshots.RemoveAt(0);
        }

        var snapshot = new AsteroidSnapshot();
        snapshot.position = transform.position;
        snapshot.rotation = transform.rotation;

        _memento.Record(snapshot);
    }

    public void Rewind()
    {
        if (!_memento.CanRemember()) return;
        if (_rigidBody.velocity != Vector2.zero)
        {
            _velocity = _rigidBody.velocity;
        }

        var snapshot = _memento.Remember();
        _rigidBody.Sleep();
        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }

    public void StopRewind()
    {
        _rigidBody.WakeUp();
        _rigidBody.velocity = _velocity;
    }

    public IEnumerator StartToRecord()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            MakeSnapshot();
        }
    }
}
