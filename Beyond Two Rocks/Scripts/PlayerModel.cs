using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public List<IWeapon> weapons;
    public float screenBoundariesX = 9.5f;
    public float screenBoundariesY = 5.5f;
    public float thrust = 6f;
    public float rotationSpeed = 180f;
    public float maxSpeed = 4.5f;
    public float invulnerableTime = 2f;
    public int health = 3;
    public AudioClip damageAudioClip;
    public AudioClip deathAudioClip;
    public AudioClip winAudioClip;
}