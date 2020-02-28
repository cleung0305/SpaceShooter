using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : TurretController
{
    public void ForceShoot(GameObject target = null, List<string> factions = null)
    {
        weaponCooldown = 0f;
        Shoot(target, factions);
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
