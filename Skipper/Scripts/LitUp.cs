using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitUp : MonoBehaviour, IObserver
{
    List<Material> _materials = new List<Material>();

    private void Awake()
    {

    }

    private void Start()
    {
        var mesh = GetComponent<MeshRenderer>();
        mesh.GetMaterials(_materials);
        var toSubscribe = FindObjectOfType<PlayerSubmarineView>();
        toSubscribe.Subscribe(this);
    }

    void LitUpPart()
    {
        foreach(var item in _materials)
        {
            item.EnableKeyword("_EMISSION");
            item.SetColor("_EmissionColor", Color.red);
        }
    }

    void TurnOffPart()
    {
        foreach (var item in _materials)
        {
            item.EnableKeyword("_EMISSION");
            item.SetColor("_EmissionColor", Color.black);
        }
    }

    public void OnNotify(string eventID)
    {
        if (eventID == "LitUpPart") LitUpPart();
        if (eventID == "TurnOffPart") TurnOffPart();
    }
}
