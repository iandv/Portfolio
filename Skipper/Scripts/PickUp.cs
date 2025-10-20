using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void ItemFound()
    {
        EventManager.Instance.Trigger("GotItem");
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemFound();
        UIQuestMarker marker = GetComponent<UIQuestMarker>();
        marker.RemoveMarker();
        Destroy(gameObject);
    }
}
