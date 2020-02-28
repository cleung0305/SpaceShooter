using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Laser : MonoBehaviour
{
    [SerializeField] float delayTime = 2f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] GameObject laser;
    [SerializeField] LaserTurret turret;
    [SerializeField] GameObject guide;

    bool charging = false;
    bool activated = false;
    float chargeTime;
    float currentLifetime;
    
    void Update()
    {
        if(charging)
        {
            chargeTime -= Time.deltaTime;
            if (chargeTime <= 0)
            {
                if (!activated)
                {
                    laser.SetActive(true);
                    guide.SetActive(false);
                }

                currentLifetime -= Time.deltaTime;

                if (currentLifetime <= 0)
                {
                    DisableLaser();
                }
            }
        }
    }

    public void EnableLaser()
    {
        charging = true;
        guide.SetActive(true);
        chargeTime = delayTime;
        currentLifetime = lifeTime;
    }

    public void DisableLaser()
    {
        laser.SetActive(false);
        activated = false;
        charging = false;
    }
}
