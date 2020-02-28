using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//This script controls the targeting for the NPC ships
public class TargetingController : MonoBehaviour
{
    [SerializeField] NpcController controller;
    [SerializeField] CircleCollider2D targetingZone;
    [SerializeField] public float radius;
    [SerializeField] List<string> enemyFactions = new List<string>();
    List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        targetingZone.radius = radius;
        if (controller == null)
            controller = GetComponentInParent<NpcController>();
    }

    public void Initialize(List<string> factions)
    {
        enemyFactions = factions;
    }

    //Activates when a collider enters the targeting zone of the ship
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipController colliderShip;
        colliderShip = collision.gameObject.GetComponent<ShipController>();

        if (colliderShip == null)
            return; //Can't target it, just exit

        if (enemyFactions.Contains(colliderShip.GetFaction()))
        {
            controller.AddTargetFromTargetingController(colliderShip);
            //Old targeting method
            //targets.Add(collision.gameObject);
            //controller.UpdateTargets(targets);
        }
    }

    public void ManualRemoveTarget(GameObject target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        string collisionFaction;
        try
        {
            collisionFaction = collision.gameObject.GetComponent<ShipController>().GetFaction();
        }
        catch //we can't target it, so just exit.
        {
            return;
        }

        if (targets.Contains(collision.gameObject))
        {
            if (!enemyFactions.Contains(collisionFaction))
            {
                targets.Remove(collision.gameObject);
            }
        }
        else
        {
            if (enemyFactions.Contains(collisionFaction))
            {
                targets.Add(collision.gameObject);
            }
        }
        controller.UpdateTargets(targets);
    }*/

    //Activates when a collider exits the targeting zone of the ship
    private void OnTriggerExit2D(Collider2D collision)
    {
        ShipController colliderShip = collision.GetComponent<ShipController>();
        if(colliderShip != null)
            controller.AttemptToRemoveTargetFromTargetingController(colliderShip);
        /*foreach (GameObject target in targets)
        {
            if (target == collision.gameObject) //if the gameobject was a target
            {
                targets.Remove(target); //remove it from the targets list
                controller.UpdateTargets(targets); //update the list of targets on the controller
                break;
            }
        }*/
    }

}
