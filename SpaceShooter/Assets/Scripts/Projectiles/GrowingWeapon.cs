using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Growing Weapon", menuName = "Weapon/Growing Weapon", order = 1)]
public class GrowingWeapon : Weapon
{

    [SerializeField] public float startSize;
    [SerializeField] public float endSize;
    [SerializeField] public float growthSpeed;
    [SerializeField] public float endDamage;
    [SerializeField] public float damagegrowthSpeed;
}
