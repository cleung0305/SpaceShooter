using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShopSlot
{

}

public class ShopState : MonoBehaviour
{
    [SerializeField] ShopControl shop; 
    [SerializeField] List<ShopSlot> availableUpgrades = new List<ShopSlot>();
    [SerializeField] List<ShopSlot> availableWeapons = new List<ShopSlot>();
    [SerializeField] List<ShopSlot> availableShips = new List<ShopSlot>();

    void EnforceState()
    {

    }

}
