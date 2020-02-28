using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointZone : MonoBehaviour
{
    [SerializeField] List<int> canHitZones = new List<int>();
    [SerializeField] List<WeakPoint> weakPoints = new List<WeakPoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       ShipController ship = collision.GetComponent<ShipController>();
        if(ship != null)
        {
            canHitZones.Add(ship.GetId());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       ShipController ship = collision.GetComponent<ShipController>();
        if(ship != null)
        {
            canHitZones.Remove(ship.GetId());
        }
        
    }

    public bool IsInZone(int id)
    {
        return canHitZones.Contains(id);
    }
}
