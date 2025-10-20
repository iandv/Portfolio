using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticMine : SeaMineBehaviour
{
    [SerializeField]
    protected float speed, detectionRange;
    [SerializeField]
    protected Transform chainOrigin;
    [SerializeField]
    protected int hardSurfaceLayerMask;

    float _chainDist;
    Vector3 _initialPos;
    Rigidbody _rb;
    MeshRenderer _mr;

    private void Awake()
    {
        _mr = GetComponentInChildren<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(chainOrigin.position.x, transform.position.y, chainOrigin.position.z);
    }

    private void Start()
    {
        _initialPos = transform.position;
        _chainDist = Vector3.Distance(_initialPos, chainOrigin.position);
    }

    void FixedUpdate()
    {
        MoveToClosestTarget();
    }

    void ChangeEmission(bool turnOn)
    {
        float eFloat = _mr.material.GetFloat("_EmissionMultiplier");
        if (turnOn && eFloat < 1f)
            _mr.material.SetFloat("_EmissionMultiplier", eFloat += Time.deltaTime);
        if (!turnOn && eFloat > 0f)
            _mr.material.SetFloat("_EmissionMultiplier", eFloat -= Time.deltaTime);
    }

    void MoveToClosestTarget()
    {
        //var overlapSphere = Physics.OverlapSphere(chainOrigin.position, _chainDist + distancePadding, hardSurfaceLayerMask);
        var overlapSphere = Physics.OverlapSphere(transform.position, detectionRange, hardSurfaceLayerMask);
        List<Transform> targets = new List<Transform>();
        Transform closest = null;
        foreach (var item in overlapSphere)
        {
            targets.Add(item.transform);
        }
        foreach (var target in targets)
        {
            bool validTarget = target.GetComponentInParent<ShipBehaviour>();
            if (validTarget)
            {
                Debug.Log("found valid");
                if (closest == null || Vector3.Distance(transform.position, target.position) < Vector3.Distance(transform.position, closest.position))
                {
                    closest = target;
                }
            }
        }


        if (closest != null)
        {
            Vector3 dir = (closest.position - transform.position).normalized;
            //_rb.velocity = Vector3.MoveTowards(transform.position, dir, speed * Time.deltaTime);
            ChangeEmission(true);
            transform.Translate(dir * speed * Time.deltaTime);
        }

        if (closest == null && Vector3.Distance(transform.position, _initialPos) > 0.1)
        {
            Vector3 dir = (_initialPos - transform.position).normalized;
            //_rb.velocity = Vector3.MoveTowards(transform.position, dir, speed * Time.deltaTime);
            ChangeEmission(false);
            transform.Translate(dir * speed * Time.deltaTime);
        }
        var tempPos = transform.position;
        tempPos.x = Mathf.Clamp(tempPos.x, chainOrigin.position.x - _chainDist, chainOrigin.position.x + _chainDist);
        tempPos.z = Mathf.Clamp(tempPos.z, chainOrigin.position.z - _chainDist, chainOrigin.position.z + _chainDist);
        transform.position = tempPos;
        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -speed, speed), Mathf.Clamp(_rb.velocity.y, -speed, speed), Mathf.Clamp(_rb.velocity.z, -speed, speed));
    }
}
