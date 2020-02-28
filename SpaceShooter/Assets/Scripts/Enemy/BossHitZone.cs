using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitZone : MonoBehaviour
{
    [SerializeField] BossController boss;

    [SerializeField] List<ShipController> shipsInHitZone = new List<ShipController>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipController ship = collision.GetComponent<ShipController>();
        if(ship != null)
        {
            if(boss.enemyFactions.Contains(ship.GetFaction()))
            {
                shipsInHitZone.Add(ship);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       ShipController ship = collision.GetComponent<ShipController>();
        if(ship != null && shipsInHitZone.Contains(ship))
            shipsInHitZone.Remove(ship);
    }


    public List<ShipController> ShipsInZone()
    {
        return shipsInHitZone;
    }
}
