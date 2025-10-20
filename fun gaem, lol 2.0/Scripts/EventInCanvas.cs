using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject newMenu;
    [SerializeField]
    private string eventName;
    void Start()
    {
        EventManager.Instance.Subscribe(eventName, Event);
    }

    void Event(params object[] parameters)
    {
        newMenu.SetActive(true);
    }
}
