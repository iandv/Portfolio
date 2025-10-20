using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerPickUp : MonoBehaviourPun
{
    public float force;
    public Transform pTransform, boxTransform, raycastOrigin;
    public bool isCarryingItem;
    public GameObject item;

    private Animator _anim;

    private void Awake()
    {
        isCarryingItem = false;
    }

    private void Update()
    {
        PickAndThrowItem();
    }

    private void PickAndThrowItem()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PickUpBox();
            }
        }
    }

    void PickUpBox()
    {
        if (!isCarryingItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f))
            {
                if (hit.transform.tag == "Item")
                {
                    photonView.RPC("RPC_PickUp", RpcTarget.AllBuffered, gameObject.GetPhotonView().ViewID, hit.transform.gameObject.GetPhotonView().ViewID);
                    _anim.SetBool("box", true);
                    Debug.Log("found box");
                }
            }
        }

        else if (isCarryingItem)
        {
            GameObject item = Helper.FindGameObjectInChildWithTag(gameObject, "MyItem");
            photonView.RPC("RPC_ThrowItem", RpcTarget.AllBuffered, gameObject.GetPhotonView().ViewID, item.gameObject.GetPhotonView().ViewID);
            _anim.SetBool("box", false);
        }
    }

    public void ReceiveAnimator(Animator anim)
    {
        _anim = anim;
    }

    [PunRPC]
    void RPC_PickUp(int senderID, int targetID)
    {
        PlayerPickUp sender = PhotonView.Find(senderID).GetComponent<PlayerPickUp>();
        Transform target = PhotonView.Find(targetID).transform;
        PhotonTransformView ptv = target.gameObject.GetComponent<PhotonTransformView>();
        ptv.enabled = true;
        target.position = sender.boxTransform.position;
        target.rotation = sender.boxTransform.rotation;
        target.parent = sender.pTransform;
        sender.item = target.gameObject;
        sender.isCarryingItem = true;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Collider col = target.GetComponent<Collider>();
        col.isTrigger = true;
        target.gameObject.tag = "MyItem";
        _anim.SetBool("box", true);
        Debug.Log("picked item");
    }

    [PunRPC]
    void RPC_ThrowItem(int senderID, int targetID)
    {
        PlayerPickUp sender = PhotonView.Find(senderID).GetComponent<PlayerPickUp>();
        Transform target = PhotonView.Find(targetID).transform;
        if (sender.isCarryingItem)
        {
            target.parent = null;
            target.position = sender.boxTransform.position;
            target.rotation = sender.boxTransform.rotation;
            PhotonTransformView ptv = target.gameObject.GetComponent<PhotonTransformView>();
            ptv.enabled = false;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Collider col = target.GetComponent<Collider>();
            col.isTrigger = false;
            target.tag = "Item";
            sender.isCarryingItem = false;
            sender.item = null;
            rb.AddForce(target.forward * force, ForceMode.Force);
            _anim.SetBool("box", false);
        }
    }
}
