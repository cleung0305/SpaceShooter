using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AggroElement
{
    public AggroElement(ShipController newShip, float agr)
    {
        ship = newShip;
        aggro = agr;
    }
    public ShipController ship;
    public float aggro;
}

public class AggroTable
{
    List<AggroElement> aggroTable = new List<AggroElement>();
    NpcController controller;

    public void Initialize(NpcController control)
    {
        controller = control;
    }

    public void ReduceAllAggro(float amount)
    {
        foreach (AggroElement element in aggroTable)
        {
            element.aggro -= amount;
        }

        aggroTable.RemoveAll(element => element.aggro < 0);
    }

    public bool AggroIsPositive(ShipController ship)
    {
        return GetElement(ship).aggro >= 0;
    }

    public void AddShip(ShipController ship, float aggro = 10)
    {
        AggroElement newEntry = new AggroElement(ship, aggro);
        aggroTable.Add(newEntry);
    }
    
    public void UpdateEntry(ShipController ship, float aggro)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                element.aggro += aggro;
        }

    }

    public bool IsInTable(ShipController ship)
    {
        foreach (AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                return true;
        }
        return false;
    }
    
    public void DropAllAggro()
    {
        aggroTable = new List<AggroElement>();
    }

    public float GetTopAggroAmount()
    {
        AggroElement topEle = GetTopAggro();
        if (topEle != null)
            return topEle.aggro;
        else return 0;
    }

    public AggroElement GetTopAggro()
    {
        if (aggroTable.Count <= 0)
            return null;
        AggroElement highestAggro = aggroTable[0];
        float distanceToAggroRatio;
        try
        {
            distanceToAggroRatio = highestAggro.aggro / Vector2.Distance(highestAggro.ship.gameObject.transform.position, controller.gameObject.transform.position);
        }
        catch
        {
            RemoveElement(highestAggro.ship);
            return null;
        }

        //Objective: find ship with highest aggro/distance ratio, if the aggro is greater than the reputation of the ship.
        //first, we just need to get the ship that has the highest aggro
        foreach (AggroElement element in aggroTable)
        {
            try
            {
                float tempDistToAggroRatio = element.aggro / Vector2.Distance(element.ship.gameObject.transform.position, controller.gameObject.transform.position);

                if (tempDistToAggroRatio >= distanceToAggroRatio) //if the ratio is higher for the new target
                {
                    highestAggro = element;
                    distanceToAggroRatio = tempDistToAggroRatio;
                }
                else if (highestAggro.aggro < Mathf.Abs(highestAggro.ship.GetReputation()) && highestAggro.ship.GetFaction() == controller.GetFaction() && element.aggro >= Mathf.Abs(highestAggro.ship.GetReputation())) //If highest aggro is less than the reputation of the ship and they are of the same faction
                {
                    highestAggro = element;
                    distanceToAggroRatio = tempDistToAggroRatio;
                }
            }
            catch
            {
                RemoveElement(element.ship);
            }
        }

        if (highestAggro.ship.GetFaction() == controller.GetFaction() && highestAggro.aggro < Mathf.Abs(highestAggro.ship.GetReputation())) //if the highest aggro is too low, just return null
            return null;

        return highestAggro;
    }

    public void RemoveElement(ShipController ship)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
            {
                aggroTable.Remove(element);
                return;
            }
        }
    }

    public AggroElement GetElement(ShipController ship)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                return element;
        }
        return null;
    }
    public List<AggroElement> GetElements()
    {
        return aggroTable;
    }
}
