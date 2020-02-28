using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class acts as a container for all of the player-related variables, such as name, reputation, and currency.
public class ShipStats : MonoBehaviour
{
    [Header("Basic Stats", order = 0)]
    [SerializeField] public int shipId = 0;
    [SerializeField] public string shipName = "Unnamed Ship";
    [SerializeField] public int level = 1;
    [SerializeField] public string faction;
    [SerializeField] public float reputation = 0;
    [SerializeField] public bool staticReputation = true;
    [SerializeField] public float speed = 5;
    [SerializeField] public float acceleration = 1;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float maxShield = 100;
    [SerializeField] public float shieldRegenRate = 10;
    [SerializeField] public float shieldRegenTime = 15;
    [SerializeField] public bool overwriteValues = true;
    [SerializeField] public bool useDefaultReputation = true;

    [Header("Upgrade", order = 1)]
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();
    [Header("Weapons", order = 2)]
    [SerializeField] public List<Weapon> weapons;
    [SerializeField] public int currentWeapon;
    [SerializeField] public List<Ship> ships;
    [SerializeField] public int currentShip;

    [Header("References", order = 3)]
    [SerializeField] public Ship ship;
    [SerializeField] public ShipController shipController;

    protected void Awake()
    {
        Initialize();
    }

    //Set up using the variables from Ship
    protected void Initialize()
    {
        if (useDefaultReputation)
            reputation = ship.defaultReputation;

        if (reputation > 10)
        {
            faction = "Federation";
        }
        else if (reputation < 10 && reputation > -1)
        {
            faction = "Neutral";
        }
        else
        {
            faction = "Pirate";
        }

        if(shipId == 0)
        {
            try
            {
                shipId = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RegisterShip();
            }
            catch
            {
                Debug.LogError("GameManager was not found in the scene!");
            }
        }

        if(ship == null)
        {
            Debug.LogWarning(shipName + " is missing a Ship ScriptableObject!");
            shipController.SetStats(this);
            return;
        }

        if (overwriteValues)
        {
            maxHealth = ship.health;
            maxShield = ship.shield;
            speed = ship.speed;
            acceleration = ship.acceleration;
            shieldRegenRate = ship.shieldRegenRate;
            shieldRegenTime = ship.shieldRegenTime;

            if (weapons == null || weapons.Count < 1)
                weapons = ship.weapons;
            else
            {
                foreach (Weapon weapon in ship.weapons)
                {
                    Debug.Log("Adding weapon" + weapon.name);
                    if (!weapons.Contains(weapon))
                        weapons.Add(weapon);
                }
            }
        }

        currentWeapon = 0;

        //Create a function like this when it is time to start changing stats based on level (player will need one to decide based on upgrades)
        //ModifyStats();

        shipController.Initialize(this);
    }

    public virtual void SetCurrentWeapon(int value)
    {
        if (value >= weapons.Count)
            currentWeapon = weapons.Count - 1;
        else if (value < 0)
            currentWeapon = 0;
        else
            currentWeapon = value;
    }

    public Weapon GetWeapon(int value)
    {
        if (weapons.Count == 0)
            return null;
        if (value >= weapons.Count)
            return weapons[weapons.Count - 1];
        else if (value < 0)
            return weapons[0];
        else
            return weapons[value];
    }

    public float GetReputation()
    {
        return reputation;
    }

    public virtual void AlterReputation(float targetRep, bool killed)
    {
        if (staticReputation)
            return;
        if(targetRep > 10) //hit a federation ship
        {
            if (killed)
                reputation -= 10;
            else
                reputation -= .1f;
        }
        else if (targetRep <= -1) //hit a pirate ship
        {
            if (killed)
                reputation += 5;
            else
                reputation += .05f;
        }
        else //hit a neutral ship
        {
            if (killed)
                reputation -= 5;
            else
                reputation -= .05f;
        }

        //make sure reputation doesn't go out of bounds
        if (reputation > 100)
            reputation = 100;
        if (reputation < -100)
            reputation = -100;

        if (reputation > 10)
            faction = "Federation";
        else if (reputation > -1)
            faction = "Neutral";
        else
            faction = "Pirate";

        shipController.UpdateFactions();

    }

    protected virtual void AddWeapon(Weapon newWeapon)
    {
        weapons.Add(newWeapon);
    }

    protected virtual void ReplaceWeapon(Weapon newWeapon)
    {
        weapons[currentWeapon] = newWeapon;
        
    }

    protected virtual void SwitchShip(Ship newShip)
    {

    }

    public void ApplyUpgrade(Upgrade newUpgrade)
    {
        upgrades.Add(newUpgrade);
        if(newUpgrade.type == 0) //hull upgrade
        {
            maxHealth += newUpgrade.increase;
        }
        else if(newUpgrade.type == 1) //shield upgrade
        {
            maxShield += newUpgrade.increase;
        }
        else if(newUpgrade.type == 2) //speed upgrade
        {
            speed += newUpgrade.increase;
        }
        else if (newUpgrade.type == 3) //new weapon
        {
            if(weapons.Count >= 5)
            {
                ReplaceWeapon(newUpgrade.weapon);
            }
            else
            {
                AddWeapon(newUpgrade.weapon);
            }
        }
        else if (newUpgrade.type == 4) // new ship
        {
            SwitchShip(newUpgrade.ship);
            ships.Add(newUpgrade.ship);
        }

        //update the controller values
        shipController.Initialize(this);
    }
}
