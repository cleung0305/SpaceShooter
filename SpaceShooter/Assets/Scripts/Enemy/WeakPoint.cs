using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : ShipController
{
    [Header("Weak Point")]
    [SerializeField] protected BossController owner;
    [SerializeField] protected WeakPointZone zone;
    [SerializeField] protected float damageToOwner;
    [SerializeField] protected Sprite damagedVersion;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoUpdateChecks();
    }

    protected override void Shoot(GameObject target = null, List<string> factions = null)
    {
        
    }

    public override bool CanBeHitBy(int id)
    {
        return zone.IsInZone(id);
    }

    protected override void PrepareForDeath()
    {
        CreateExplosion();
        maxShield = 0f;
        shieldRegenRate = 0f;
        spriteRenderer.sprite = damagedVersion;
        shipCollider.enabled = false;
        owner.TakeDamage(damageToOwner, GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>());
        audioSource.clip = deathSound;
        audioSource.Play();
    }

    protected override void Die()
    {
        //do nothing
    }
}
