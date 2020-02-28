using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seeking Weapon", menuName = "Weapon/Seeking Weapon", order = 3)]
public class SeekingWeapon : Weapon
{
    [SerializeField] public float lifespan = 5;
    [SerializeField] public float followRange = 15;
    [SerializeField] public float followStrength = .5f;
    [SerializeField] public float absoluteTrackingTime = .5f;
    [SerializeField] public float flyStraightTime = .25f;
    [SerializeField] public float turnSpeed = 5;
    [SerializeField] public float maxTurnSpeed = 15;
    [SerializeField] public float speedGrowth = 1;
    [SerializeField] public float maxSpeed = 5;
    [SerializeField] public AudioClip explosionSound;
}
