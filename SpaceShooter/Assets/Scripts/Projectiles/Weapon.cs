using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject that acts as a container for weapon stats
[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Basic Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [SerializeField] public float damage = 1;
    [SerializeField] public float fireRate = 1;
    [SerializeField] public float projectileSpeed = 1;
    [SerializeField] public Sprite projectileImage;
    [SerializeField] public float npcFollowDistance = 25f;
    [SerializeField] public float range = 25f;
    [SerializeField] public AudioClip sound;
    [Range(0.0f, 1f)]
    [SerializeField] public float volume = 1f;
    [SerializeField] public GameObject projectile;
    [SerializeField] public Vector2 scale = new Vector2(1,1);
    [SerializeField] public float degreesOfAccuracy = 45f;
    [SerializeField] public bool canBeHitByProjectiles = false;
    [SerializeField] public float damageBeforeDestroyed = 1f;

}
