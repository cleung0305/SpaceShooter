using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : TurretController
{
    [SerializeField] Boss3Laser laser;
    [SerializeField] float fireCD;

    bool firing = false;
    float currentCd = 15;
    float followTime;
    protected override void FixedUpdate()
    {
        currentCd -= Time.deltaTime;
        followTime -= Time.deltaTime;
        if (currentCd <= 0)
            firing = false;

        if(!firing || followTime >= 0)
            TurretRotate();
    }

    public void SetTarget(ShipController target)
    {
        currentTarget = target;
    }

    protected override bool TargetInAttackRange()
    {
        return true;
    }

    protected override void Shoot(GameObject target = null, List<string> factions = null)
    {
        //do nothing
    }

    protected override void ResetRotation()
    {
        if(!firing)
        {
            base.ResetRotation();
        }
    }

    public bool IsFollowingTarget()
    {
        return TargetWithinShootAngle();
    }

    public bool FireLaser(float cd = 15)
    {
        if (!TargetWithinShootAngle())
            return false;
        firing = true;
        followTime = 1.5f;
        currentCd = cd;
        laser.EnableLaser();
        return true;
    }
}
