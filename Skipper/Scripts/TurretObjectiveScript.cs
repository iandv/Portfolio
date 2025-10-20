using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretObjectiveScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.TurretObjective();
            Destroy(gameObject);
        }
    }
}
