using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class Ship : ScriptableObject
{
    public Sprite baseSprite;
    public RuntimeAnimatorController animatorController;
    public GameObject explosion;

    [Header("Basic Ship Variables")]
    public int health;
    public int shield;
    public float shieldRegenRate;
    public float shieldRegenTime;

    [Header("Weapons")]
    public List<Weapon> weapons;

    [Header("Movement Variables")]
    public float speed;
    public float acceleration = 1;
    public float mass;
    public float drag;
    public float turnSpeed;

    [Header("Reputation")]
    public float defaultReputation;
}
