using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public WeightScript.Color selectedColor;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            if (collision.gameObject.GetComponent<WeightScript>().myColor == selectedColor)
            {
                EventManager.Instance.Trigger("PressedPlate");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            if (collision.gameObject.GetComponent<WeightScript>().myColor == selectedColor)
            {
                EventManager.Instance.Trigger("LeftPlate");
            }
        }
    }
}
