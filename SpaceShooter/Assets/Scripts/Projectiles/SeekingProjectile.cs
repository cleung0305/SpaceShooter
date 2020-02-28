using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : Projectile
{
    float lifespan;
    float followRange;
    float followStrength;
    float turnSpeed;
    float maxTurnSpeed;
    float flyStraightTime;
    float missileSpeed;
    float maxSpeed;
    float speedGrowth;
    AudioClip explosionSound;
    bool flyingStraight = true;
    float absoluteTrackingTime;
    float currentLifeSpan = 0f;
    Rigidbody2D missileRb;

    float distanceToTarget;
    [SerializeField] float angleToTarget;
    [SerializeField] Animator thrusterEffect;
    float lastDistanceToTarget = 1000f;
    float turnSpeedIncrease = .1f;
    public override void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        base.Initialize(weapon, shooter, target, factions);
        SeekingWeapon seekingWeapon = (SeekingWeapon)weapon;
        lifespan = seekingWeapon.lifespan;
        followRange = seekingWeapon.followRange;
        followStrength = Mathf.Abs(seekingWeapon.followStrength * 90);
        absoluteTrackingTime = seekingWeapon.absoluteTrackingTime;
        turnSpeed = seekingWeapon.turnSpeed;
        maxTurnSpeed = seekingWeapon.maxTurnSpeed;
        flyStraightTime = seekingWeapon.flyStraightTime;
        missileSpeed = seekingWeapon.projectileSpeed;
        speedGrowth = seekingWeapon.speedGrowth;
        maxSpeed = seekingWeapon.maxSpeed;
        explosionSound = seekingWeapon.explosionSound;
        missileRb = GetComponent<Rigidbody2D>();
        if(thrusterEffect != null)
            thrusterEffect.SetBool("IsOn", true);
    }

    protected override void Update()
    {
        if (target != null)
        {
            angleToTarget = AngleToTarget();
            distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        }

        currentLifeSpan += Time.deltaTime;

        if (target != null && currentLifeSpan >= absoluteTrackingTime && (Vector2.Distance(transform.position, target.transform.position) > followRange || (angleToTarget < 90 - followStrength || angleToTarget > 90 + followStrength)))
        {
            target = null;
            flyingStraight = true;
        }

        if (flyingStraight && currentLifeSpan > flyStraightTime && target != null)
            flyingStraight = false;

        if(!flyingStraight)
        {
            Vector2 direction = transform.position - target.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
            turnSpeed = AdjustedTurnSpeed(turnSpeed, distanceToTarget, maxTurnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }

        missileSpeed = missileSpeed >= maxSpeed ? maxSpeed : missileSpeed + speedGrowth * Time.deltaTime;
        missileRb.AddForce(transform.up * missileSpeed * 100 * missileRb.drag * missileRb.mass * Time.deltaTime);

        if (waitingForDestroy)
        {
            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        lastDistanceToTarget = distanceToTarget;
    }

    float AdjustedTurnSpeed(float speed, float distance, float max = -1)
    {
        float tempSpeed = turnSpeed;
        if (Mathf.Abs(lastDistanceToTarget - distance) < .2 && distance < 2)
        {
            turnSpeedIncrease += 5 * Time.deltaTime;
            tempSpeed += tempSpeed * turnSpeedIncrease * Time.deltaTime;
        }
        return tempSpeed > max && max > 0 ? max : tempSpeed;
    }

    protected float AngleToTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= transform.rotation.eulerAngles.z;
        return angle < 0 ? angle + 360 : angle;
    }

    protected override void LateUpdate()
    {
        if (currentLifeSpan > lifespan && !waitingForDestroy)
        {
            Deactivate();
        }
    }

    protected override void Deactivate()
    {
        audioSource.clip = explosionSound;
        audioSource.Play();
        base.Deactivate();
        if(thrusterEffect != null)
            thrusterEffect.gameObject.SetActive(false);
        if (onHitExplosion != null)
        {
            GameObject explosion = Instantiate(onHitExplosion, transform.position, transform.rotation);
            explosion.transform.localScale = transform.localScale;
        }

    }

}
