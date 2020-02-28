using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingMissileLauncher : TurretController
{
    Boss2Controller boss2;
    private void Awake()
    {
        base.Awake();
        aggroTable.Initialize(this);
        targetingController.Initialize(enemyFactions);
    }

    int shootCount = 0;
    // Update is called once per frame
    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
    }

    public override void SetBoss(BossController boss)
    {
        stats.shipId = boss.GetStats().shipId;
        this.boss = boss;
        boss2 = (Boss2Controller) boss;
    }

    void Update()
    {
        weaponCooldown -= Time.deltaTime;
        if (currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            targetPosition = currentTarget.transform.position;
            if (OutOfRange() || currentTarget.GetComponent<ShipController>().IsDead())
            {
                Deaggro();
            }
            if(currentTarget != null && TargetInWeaponRange() && weaponCooldown <= 0 && !isDead && TargetWithinShootAngle())
            {
                Shoot(currentTarget.gameObject);
                shootCount += 1;
                if(shootCount == 3)
                {
                    shootCount = 0;
                    weaponCooldown = 25f;
                }
            }
        }
    }
}
