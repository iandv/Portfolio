using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    PlayerSubmarineModel _psm;
    Transform _item;
    bool _canPickUp = true;

    private void Start()
    {
        _psm = GetComponentInParent<PlayerSubmarineModel>();
    }

    private void Update()
    {
        UnHookItem();
    }

    private void HookItem(Transform item)
    {
        if (item != null)
        {
            _item = item;
            Rigidbody rb = _item.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            _item.SetParent(this.transform);
            _item.position = transform.position;
            _item.rotation = transform.rotation;
            _psm.ActionButtonMessage.SetActive(false);
            StartCoroutine(WaitUp());
        }
    }

    private void UnHookItem()
    {
        if (_item != null)
        {
            if (Input.GetKey(KeyCode.E) && !_canPickUp)
            {
                _item.parent = null;
                Rigidbody rb = _item.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                _item = null;
                StartCoroutine(WaitUp());
            }
        }
    }

    IEnumerator WaitUp()
    {
        yield return new WaitForSeconds(0.1f);
        _canPickUp = !_canPickUp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PickUp" && _item == null)
        {
            _psm.ActionButtonMessage.SetActive(true);
            if (Input.GetKey(KeyCode.E) && _canPickUp)
            {
                Transform item = other.transform;
                HookItem(item);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
            _psm.ActionButtonMessage.SetActive(false);

    }
}
