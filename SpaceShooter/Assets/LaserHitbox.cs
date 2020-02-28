using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    [SerializeField] float tickTime;
    [SerializeField] float damage;
    [SerializeField] ShipController boss;
    float hitTimer;
    // Start is called before the first frame update
    void Start()
    {
        hitTimer = tickTime;
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipController ship = collision.GetComponent<ShipController>();
        if (ship != null)
        {
            ship.TakeDamage(damage, null);
            hitTimer = tickTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ShipController ship = collision.GetComponent<ShipController>();
        if (ship != null && hitTimer <= 0)
        {
            ship.TakeDamage(damage, null);
            hitTimer = tickTime;
        }
    }
}
