using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : BossController
{
    [SerializeField] float missileCD = 30f;
    [SerializeField] WeaponHolder forwardMissileL;
    [SerializeField] WeaponHolder forwardMissileR;
    [SerializeField] BossHitZone hitZone2;

    float leftCD, rightCD = 0f;

    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
        aggroTable.Initialize(this);
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;
        foreach (Transform turret in turretContainer.transform)
        {
            TurretController turretController = turret.GetComponentInChildren<TurretController>();
            turretController.SetBoss(this);
            turrets.Add(turretController);
        }
        aggroTable.AddShip(GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>(), 100);
        aggroElements = aggroTable.GetElements();
        UpdateFactions();
    }

    protected override void DoUpdateChecks()
    {
        base.DoUpdateChecks();
        weaponCooldown = leftCD > rightCD ? leftCD : rightCD;
        leftCD -= Time.deltaTime;
        rightCD -= Time.deltaTime;
    }


    protected override bool TurretsInRange()
    {

        foreach (TurretController turret in turrets)
        {
            if (turret.CanHitTarget() && turret.gameObject.name != "BossMainMissile")
            {
                return true;
            }
        }
        return false;

    }

    protected override void Shoot(GameObject target = null, List<string> factions = null)
    {
        List<ShipController> shipsinLZone = hitZone.ShipsInZone();
        List<ShipController> shipsInRZone= hitZone2.ShipsInZone();

        //The target is in one of the zones
        if (shipsInRZone.Contains(currentTarget))
        {
            if (rightCD <= 0)
            {
                StartCoroutine("DoAnim", 1);
            }
        }
        else if (shipsinLZone.Contains(currentTarget))
        {
            if(leftCD <= 0)
                StartCoroutine("DoAnim", 0);
        }
    }

    IEnumerator DoAnim(int side)
    {
        if (side == 0)
        {
            leftCD = missileCD;
            rightCD = missileCD * .5f;
        }
        else
        {
            rightCD = missileCD;
            leftCD = missileCD * .5f;
        }
        StallTime(5);
        shipAnimator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(2);
        if(side == 0)
            forwardMissileL.ForceShoot(currentTarget.gameObject, enemyFactions);
        else
            forwardMissileR.ForceShoot(currentTarget.gameObject, enemyFactions);
        shipAnimator.SetBool("IsOpen", false);
        shipAnimator.SetTrigger("Close");
    }

}
