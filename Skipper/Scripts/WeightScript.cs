using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScript : MonoBehaviour
{
    public Color myColor;
    public enum Color
    {
        Green,
        Yellow,
        Purple
    }

    private void Start()
    {
        GameManager.Instance.AddCountToNeeded();
    }
}
