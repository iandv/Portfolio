using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Player : MonoBehaviourPun
{
    [SerializeField]
    private float speed, jumpForce, gravity = 9.81f;
    [SerializeField]
    private GameObject meshRendererOne, meshRendererTwo;

    private float _directionY;
    public bool endGame;
    private Animator _anim;
    private Transform _spawnPosition;
    private Collider _col;
    private CharacterController _cc;
    private int _materialNum;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _col = GetComponent<Collider>();
        endGame = false;
    }

    void Start()
    {
        GameManager.instance.JoinGame(this);
        EventManager.Instance.Subscribe("Win", EndGame);
        EventManager.Instance.Subscribe("Lose", EndGame);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine || endGame) return;
        Movement();
        Jump();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveVector = (transform.forward * speed * Time.deltaTime * z) + (transform.right * speed * Time.deltaTime * x);
        _cc.Move(moveVector);
        _anim.SetFloat("run", moveVector.magnitude);
    }

    private void Jump()
    {
        Vector3 velY = new Vector3(0, _directionY, 0);
        _cc.Move(velY * Time.deltaTime);
        if (_cc.isGrounded) _anim.SetBool("fall", false);

        if (_cc.isGrounded && Input.GetKey(KeyCode.Space))
        {
            _anim.SetTrigger("jump");
            _directionY = jumpForce;
        }
        if (!_cc.isGrounded)
        {
            _anim.SetBool("fall", true);
            _directionY -= gravity * Time.deltaTime;
        }
    }

    private void EndGame(params object[] parameters)
    {
        endGame = true;
    }

    public void ChangePosition(Transform t)
    {
        if (photonView.IsMine)
        {
            _spawnPosition = t;
            transform.position = t.position;
            _cc.enabled = true;
        }


        Debug.Log("Changed Position");
    }

    public void ChangeSkin(int i)
    {
        meshRendererOne.SetActive(true);
        meshRendererTwo.SetActive(false);
        _anim = meshRendererOne.GetComponent<Animator>();

        if (photonView.IsMine && i == 2)
        {
            meshRendererOne.SetActive(false);
            meshRendererTwo.SetActive(true);
            _anim = meshRendererTwo.GetComponent<Animator>();
        }
        photonView.RPC("RPC_ChangeSkin", RpcTarget.MasterClient);

        PlayerPickUp ppu = GetComponentInChildren<PlayerPickUp>();
        ppu.ReceiveAnimator(_anim);
    }

    [PunRPC]
    void RPC_ChangeSkin()
    {
        if (!photonView.IsMine)
        {
            meshRendererOne.SetActive(false);
            meshRendererTwo.SetActive(true);
            _anim = meshRendererTwo.GetComponent<Animator>();
            PlayerPickUp ppu = GetComponentInChildren<PlayerPickUp>();
            ppu.ReceiveAnimator(_anim);
        }
    }
}
