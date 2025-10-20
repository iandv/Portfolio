using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody hook;

    [SerializeField]
    private ConfigurableJoint linkPrefab;

    [SerializeField]
    int numberOfLinks = 1;

    private void Start()
    {
        GenerateChain();
    }

    void GenerateChain()
    {
        Rigidbody previousLink = hook;

        for (int i = 0; i < numberOfLinks; i++)
        {
            ConfigurableJoint link = Instantiate(linkPrefab, transform);
            link.connectedBody = previousLink;

            previousLink = link.GetComponent<Rigidbody>();


        }
    }
}
