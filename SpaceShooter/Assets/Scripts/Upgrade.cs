using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public int type;
    public int maxBuyCount;
    public string upgradeName;
    public string description;
    public int cost;
    public float increase;
    public Weapon weapon;
    public Ship ship;
    // 0 is hull, 1 is shield, 2 is speed, 3 weapon

}
