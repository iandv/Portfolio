using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool<T>
{
    private List<T> _unistantiated = new List<T>();
    private IFactory<T, float> _factory;
    private float _value;
    private Func<T, T> _turnOn;
    private Func<T, T> _turnOff;

    public ObjectPool(int amount, IFactory<T,float> factory, float value, Func<T, T> turnOn, Func<T, T> turnOff)
    {
        _factory = factory;
        _value = value;
        _turnOn = turnOn;
        _turnOff = turnOff;
        for (var i = 0; i < amount; i++)
        {
            var obj = _factory.Create(value);
            _turnOff(obj);
            _unistantiated.Add(obj);
        }
    }

    public T Get()
    {
        T obj;

        if (_unistantiated.Count > 0)
        {
            obj = _unistantiated[0];
            _unistantiated.Remove(obj);
        }

        else
        {
            obj = _factory.Create(_value);
        }

        _turnOn(obj);

        return (obj);
    }

    public void Return(T obj)
    {
        _unistantiated.Add(obj);
        _turnOff(obj);
    }
}
