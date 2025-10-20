using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateScript : MonoBehaviour
{
    Animator _anim;
    bool _open, _promptShown;
    Collider _col;
    int _counter;

    [SerializeField]
    Text prompt;
    [SerializeField]
    string textOnPrompt;
    [SerializeField]
    List<GameObject> tablets;
    [SerializeField]
    float delay;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _col = GetComponent<Collider>();
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("Open", GateBool);
    }

    void GateBool(params object[] parameters)
    {
        _open = true;
    }

    void OpenGate()
    {
        InvokeRepeating("ShowTablets", 0, delay);
        _col.enabled = false;
    }

    void ShowTablets()
    {
        if (_counter < tablets.Count)
        {
            tablets[_counter].SetActive(true);
            _counter++;
        }

        else
        {
            CancelInvoke("ShowTablets");
            _anim.SetTrigger("Open");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _open)
        {
            OpenGate();
        }

        if (other.tag == "Player" && !_open)
        {
            prompt.text = textOnPrompt;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !_open & !_promptShown)
        {
            prompt.text = "";
        }
    }
}
