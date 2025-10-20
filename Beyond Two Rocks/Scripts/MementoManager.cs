using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoManager : MonoBehaviour
{
    public static MementoManager instance;
    
    private List<IReminder> _reminders = new List<IReminder>();   
    private List<Coroutine> _recordCoroutines = new List<Coroutine>();
    private Coroutine _rememberCoroutine;
    private bool _isRemembering;

    private void Awake() 
    {
        instance = this;
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("Rewind", Rewind);
        OEventManager.Instance.Subscribe("StopRewind", StopRewind);
    }

    public void Add(IReminder reminder) 
    {
        _reminders.Add(reminder);
        
        var coroutine = StartCoroutine(reminder.StartToRecord());
        _recordCoroutines.Add(coroutine);
    }

    private void Rewind(params object[] parameters)
    {
        _isRemembering = true;
        _rememberCoroutine = StartCoroutine(Remember());
        StopRecording();
    }

    private void StopRewind(params object[] parameters)
    {
        foreach (var reminder in _reminders)
        {
            reminder.StopRewind();
        }
        _isRemembering = false;
        StopCoroutine(_rememberCoroutine);
        StartRecording();
    }

    private void StartRecording() 
    {
        foreach (var reminder in _reminders) 
        {
            var coroutine = StartCoroutine(reminder.StartToRecord());
            _recordCoroutines.Add(coroutine);
        }
    }
    
    private void StopRecording() 
    {
        while (_recordCoroutines.Count > 0) 
        {
            StopCoroutine(_recordCoroutines[0]);
            _recordCoroutines.RemoveAt(0);
        }
    }
    
    private IEnumerator Remember() 
    {
        while (_isRemembering) 
        {
            foreach (var reminder in _reminders) 
            {
                reminder.Rewind();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}