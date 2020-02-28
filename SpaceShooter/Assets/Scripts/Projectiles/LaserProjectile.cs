using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : Projectile
{
    float offsetDistance;
    float startLength;
    float maxLength;
    float growSpeed;
    float widthGrowth;
    float lengthGrowth;

    Vector3 defaultSize;
    float size;
    private void Awake()
    {
        startPos = transform.position;
    }

    public override void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        base.Initialize(weapon, shooter, target, factions);
        LaserWeapon laserWeapon = (LaserWeapon)weapon;
        startLength = laserWeapon.startLength;
        maxLength = laserWeapon.maxLength;
        growSpeed = laserWeapon.growSpeed;
        widthGrowth = laserWeapon.widthGrowth;
        lengthGrowth = laserWeapon.lengthGrowth;
        offsetDistance = laserWeapon.offsetDistance;
        transform.SetParent(shooter.gameObject.transform);

        defaultSize = new Vector3(1, startLength, 1);
    }

    protected override void Update()
    {
        base.Update();
    }
    /*protected override void Update()
    {
        base.Update();
        size += growSpeed * Time.deltaTime;
        if (size > maxLength)
            size = maxLength;
        transform.localScale = new Vector3(5, size, 0);
        //hitBox.size. = Vector2.Distance(renderer.bounds.min, renderer.bounds.max);
        transform.localPosition = new Vector3(0, offsetDistance + Vector2.Distance(renderer.bounds.min, renderer.bounds.max) / 2, 0f);
        renderer.enabled = true;
    }*/

    private void LateUpdate()
    {
        
    }
}
