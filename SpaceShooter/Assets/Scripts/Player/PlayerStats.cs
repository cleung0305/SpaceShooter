using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ShipStats
{ 
    [Header("Player Specific", order = 1)]
    [SerializeField] int experience;
    [SerializeField] int units;
    [SerializeField] PlayerStatsUI statsUI;
    [SerializeField] List<Sprite> factionSprites = new List<Sprite>();
    [SerializeField] List<WeaponUI> weaponHolders = new List<WeaponUI>();

    [SerializeField] List<int> purchasedUpgrades = new List<int>();
    bool uiIsActive = false;
    PlayerController pc;

    private void Awake()
    {
        //units = 15000;
        base.Awake();
        staticReputation = false;
        pc = (PlayerController)shipController;

        shipId = 0;

        for(int i = 0; i < weapons.Count; i++)
        {
            weaponHolders[i].SlotImage(weapons[i].projectileImage);
        }
    }

    public void AddExperience(int exp)
    {
        experience += exp;
        if (experience >= level * 50) //player levels up
        {
            experience -= level * 50;
            level += 1;
        }
    }

    public int GetUnits()
    {
        return units;
    }

    public void ModifyUnits(int amount)
    {
        units += amount;
    }

    public void ResetUnits(int amount)
    {
        units = amount;
    }

    public override void AlterReputation(float targetRep, bool killed)
    {
        base.AlterReputation(targetRep, killed);
    }

    public void SetReputation(float rep)
    {
        reputation = rep;
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

    protected override void AddWeapon(Weapon newWeapon)
    {
        weaponHolders[weapons.Count].SlotImage(newWeapon.projectileImage);
        base.AddWeapon(newWeapon);
    }

    protected override void ReplaceWeapon(Weapon newWeapon)
    {
        weaponHolders[currentWeapon].SlotImage(newWeapon.projectileImage);
        base.ReplaceWeapon(newWeapon);
    }

    public override void SetCurrentWeapon(int value)
    {
        base.SetCurrentWeapon(value);
        if(value < weapons.Count)
        {
            weaponHolders[value].GetSelected();
            for (int i = 0; i < weaponHolders.Count; i++)
            {
                if (i != value)
                {
                    weaponHolders[i].AccelerateFade();
                }
            }
            pc.SetDelay(value, .5f);
        }
    }

    private void Update()
    {
        if (uiIsActive)
            DisplayPlayerStatsUI();
    }

    public void ToggleUI()
    {
        if (uiIsActive)
            statsUI.Hide();
        else
            DisplayPlayerStatsUI();
        uiIsActive = !uiIsActive;
    }

    protected override void SwitchShip(Ship newShip)
    {
        ship = newShip;
        Initialize();
        weapons = new List<Weapon>();
        AddWeapon(ship.weapons[0]);
    }

    void DisplayPlayerStatsUI()
    {
        int currentHealth = (int)pc.GetCurrentHealth();
        int currentShield = (int)pc.GetCurrentShield();

        string healthTxt = currentHealth.ToString() + " / " + maxHealth.ToString();
        string shieldTxt = currentShield.ToString() + " / " + maxShield.ToString();
        string speedTxt = speed.ToString();
        Sprite factionSprite;
        if (reputation <= -1)
            factionSprite = factionSprites[2];
        else if (reputation > 10)
            factionSprite = factionSprites[1];
        else
            factionSprite = factionSprites[0];
        statsUI.UpdateDisplay(ship.baseSprite, factionSprite, healthTxt, shieldTxt, speedTxt, faction, ((int)reputation).ToString(), units.ToString(), false, false, false);
    }

    public void AquireHyperDrivePart(int partNum)
    {
        statsUI.AquireHyperDrivePart(partNum);
    }
}
