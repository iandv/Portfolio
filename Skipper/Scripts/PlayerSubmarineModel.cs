using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSubmarineModel : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    public float maxSurfaceSpeed, maxUnderWaterSpeed, thrust, rotationSpeed, surfaceDistance, maxBatteryCharge, rechargeModifier = 1f, noEnergySpeed, damageFeedback = 2f;
    public int maxHealth;
    public bool onSurface;
    public Transform surfaceCheck;
    public LayerMask layerMask;
    public GameObject deathScreen, UpWarning, ActionButtonMessage;
    public Image batteryMeter;
    public AudioClip movementSounds;
    public Text lifeCounter;
}
