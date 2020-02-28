using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Targeting")]
    [SerializeField] ShipController shooter;
    [SerializeField] List<string> enemyFactions;
    [SerializeField] protected GameObject target;

    [Header("References")]
    [SerializeField] Weapon weapon;
    [SerializeField] protected BoxCollider2D hitBox;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected AudioSource audioSource;
    float speedMod = 0f;

    float health = 1f;
    protected float damage;
    protected bool waitingForDestroy = false;
    protected Vector2 startPos;

    [SerializeField] protected GameObject onHitExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
    }

    //Used to set up some information that the projectile needs
    public virtual void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        this.weapon = weapon;
        damage = weapon.damage;
        renderer.sprite = weapon.projectileImage;
        
        this.shooter = shooter;
        enemyFactions = factions;
        this.target = target;

        if (weapon.sound != null)
        {
            audioSource.clip = weapon.sound;
            audioSource.volume = weapon.volume;
            audioSource.Play();
        }

        transform.localScale = weapon.scale;
        Vector2 shooterSpeed = shooter.GetSpeed();
        speedMod = shooterSpeed.magnitude;

        hitBox.size = new Vector2(hitBox.size.x * weapon.scale.x, hitBox.size.y * weapon.scale.y);


        health = weapon.damageBeforeDestroyed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(waitingForDestroy)
        {
            if(!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += (transform.up * (weapon.projectileSpeed + speedMod) * Time.deltaTime);
        }
        
    }

    //called after physics updates
    protected virtual void LateUpdate()
    {
        if (Vector2.Distance(transform.position, startPos) >= weapon.range)
            Deactivate();
    }

    //called when the projectile collides with something
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid") //if it's an asteroid, we know what to do
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
            Deactivate();
        }
        else
        {
           ShipController colliderShip = collision.gameObject.GetComponent<ShipController>(); //first make sure that what we're colliding with is indeed a ship
            if (colliderShip != null) //if it got aShipControllerscript, it must be a ship
            {
                string colliderFaction = colliderShip.GetFaction(); //get the faction of the colliding ship
                if (enemyFactions.Contains(colliderFaction) || colliderShip.gameObject == target) //if it's in the list of enemy factions (factions that the projectile can hit)
                {
                    if (colliderShip.CanBeHitBy(shooter.GetId()))
                    {
                        colliderShip.TakeDamage(damage, shooter);
                        Deactivate();
                        if (collision.gameObject.GetComponent<ShipController>().getShield() < 1)
                        {
                            OnHit();
                        }
                    }
                }
            }
            else
            {
                //see if it's a missile
                Projectile proj = collision.gameObject.GetComponent<Projectile>();
                if (proj == null)
                    return;
                Weapon wep = proj.weapon;
                if (wep != null && wep.canBeHitByProjectiles && shooter.GetId() != proj.GetShooterId())
                {
                    proj.GetHitByOtherProjectile(damage);
                    Deactivate();
                    OnHit();
                }
            }

        }
    }

    public void GetHitByOtherProjectile(float damage)
    {
        health -= damage;
        if(health <= 0)
            Deactivate();
    }

    public int GetShooterId()
    {
        return shooter.GetId();
    }
    //Called when it is time for the projectile to die. Makes it so nothing can interact with it, but sets waitingForDestroy to true so we can finish hearing the sound
    protected virtual void Deactivate()
    {
        hitBox.enabled = false;
        renderer.enabled = false;
        waitingForDestroy = true;
    }

    protected void OnHit()
    {
        if(onHitExplosion != null)
            Instantiate(onHitExplosion, transform.position, transform.rotation);
    }
}
