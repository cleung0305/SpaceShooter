using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : BossController
{

    [SerializeField] Boss3Shielding leftShielding;
    [SerializeField] Boss3Shielding rightShielding;
    [SerializeField] LaserTurret leftLaser;
    [SerializeField] LaserTurret rightLaser;

    [SerializeField] float laserCD = 35;
    [SerializeField] float spawnCD = 45;

    float leftSpawnTimer;
    float rightSpawnTimer;
    float leftLaserCD;
    float rightLaserCD;

    bool firedLaser = false;
    bool spawnedShip = false;
    List<GameObject> spawnableShips = new List<GameObject>();

    private void Start()
    {
        leftSpawnTimer = 17f;
        rightSpawnTimer = 17f;
        leftLaserCD = 17f;
        rightLaserCD = 17f;
    }

    private void Update()
    {
        rightLaserCD -= Time.deltaTime;
        leftLaserCD -= Time.deltaTime;
        leftSpawnTimer -= Time.deltaTime;
        rightSpawnTimer -= Time.deltaTime;

        //spawnships
        if(leftSpawnTimer <= 0)
        {
            spawnedShip = SpawnShip(0);
            leftSpawnTimer = spawnCD + Random.Range(0f, .5f) * spawnCD;
        }
        
        if(rightSpawnTimer <= 0)
        {
            spawnedShip = SpawnShip(1);
            rightSpawnTimer = spawnCD + Random.Range(0f, .5f) * spawnCD;
        }

        if (spawnedShip)
        {
            StallTime(7);
            spawnedShip = false;
            if(leftLaserCD < 7)
                leftLaserCD = 7;
            if(rightLaserCD < 7)
                rightLaserCD += 7;
        }

        //fire lasers
        if (leftLaserCD <= 0)
        {
            if(leftLaser.FireLaser(5))
            {
                leftLaserCD = laserCD;
                firedLaser = true;
            }
        }

        if (rightLaserCD <= 0)
        {
            if (rightLaser.FireLaser(5))
            {
                rightLaserCD = laserCD;
                firedLaser = true;
            }
        }


        if (firedLaser)
        {
            StallTime(4);
            firedLaser = false;
        }

        if(currentTarget != null)
        {
            leftLaser.SetTarget(currentTarget);
            rightLaser.SetTarget(currentTarget);
        }

    }


    bool SpawnShip(int side)
    {
        if (side == 0)
        {
            if (!leftShielding.IsDestroyed())
            {
                leftShielding.Open();
                return true;
            }
        }
        else
        {
            if (!rightShielding.IsDestroyed())
            {
                rightShielding.Open();
                return true;
            }
        }

        return false;
    }
}
