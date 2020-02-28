using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingProjectile : Projectile
{
    float startSize;
    float endSize;
    float growthSpeed;
    float endDamage;
    float damagegrowthSpeed;

    public override void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        base.Initialize(weapon, shooter, target, factions);
        GrowingWeapon grWeapon = (GrowingWeapon)weapon;
        startSize = grWeapon.startSize;
        gameObject.transform.localScale += new Vector3(startSize - 1, startSize - 1, 1);
        endSize = grWeapon.endSize;
        growthSpeed = grWeapon.growthSpeed;
        endDamage = grWeapon.endDamage;
        damagegrowthSpeed = grWeapon.damagegrowthSpeed;

    }

    protected override void Update()
    {
        base.Update();
        Scale();
        if(endDamage > damage)
        {
            ScaleDamage();
        }
    }

    void Scale()
    {
        float size = gameObject.transform.localScale.x;
        if(size < endSize)
        {
            size += growthSpeed * Time.deltaTime;
            gameObject.transform.localScale = new Vector3(size, size, 1);
        }
        else
        {
            transform.localScale = new Vector3(endSize, endSize, 1);
        }
    }

    void ScaleDamage()
    {
        damage += damagegrowthSpeed * Time.deltaTime;
    }
}
