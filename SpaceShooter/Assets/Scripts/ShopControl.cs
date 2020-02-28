using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopControl : MonoBehaviour
{
    int units;
    int isSold;

    public TextMeshProUGUI unitsText;
    public TextMeshProUGUI weaponText;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject shop2;
    [SerializeField] private List<GameObject> upgradeShopUI;
    [SerializeField] private List<GameObject> shipShopUI;
    [SerializeField] private List<GameObject> itemDescriptions;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public Button buy;

    // Start is called before the first frame update
    void Start()
    {
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();

        foreach (var obj in shipShopUI)
            obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //unitsText.text = "Units: " + playerStats.GetUnits().ToString();
        /*isSold = PlayerPrefs.GetInt("IsSold");

        if (units >= 5 && isSold == 0)
            buy.interactable = true;
        else
            buy.interactable = false;*/
    }

    public void buyUpgrade(ShopButton clickedButton)
    {
        if (units >= clickedButton.upgrade.cost)
        {
            playerStats.ModifyUnits(-clickedButton.upgrade.cost);
            playerStats.ApplyUpgrade(clickedButton.upgrade);
            clickedButton.GetPurchased();
        }
        else
        {
            UpdateDescriptionDisplay(clickedButton);
        }
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    public void buyWeapon(ShopButton clickedButton)
    {
        if(units >= clickedButton.upgrade.cost)
        {
            playerStats.ModifyUnits(-clickedButton.upgrade.cost);
            playerStats.ApplyUpgrade(clickedButton.upgrade);
            clickedButton.GetPurchased();
        }
        else
        {
            ForceDescriptionUpdate("Not Enough Credits!");
        }
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    public void buyShip(ShopButton clickedButton)
    {
        if (units >= clickedButton.upgrade.cost)
        {
            playerStats.ModifyUnits(-clickedButton.upgrade.cost);
            playerStats.ApplyUpgrade(clickedButton.upgrade);
            clickedButton.GetPurchased();
            ResetShops();
        }
        else
        {
            ForceDescriptionUpdate("Not Enough Credits!");
        }
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    public void RepairHull()
    {
        if (units >= 50)
        {
            playerStats.gameObject.GetComponent<PlayerController>().RepairHull();
            playerStats.ModifyUnits(-50);
        }
        else
        {
            ForceDescriptionUpdate("Not Enough Credits!");
        }
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    public void ResetShops()
    {
        GameObject[] shops = GameObject.FindGameObjectsWithTag("Shop");
        foreach(GameObject shop in shops)
        {
            shop.GetComponentInChildren<ShopControl>().Restock();
        }
    }

    public void Restock()
    {
        foreach (GameObject obj in upgradeShopUI)
        {
            ShopButton btn = obj.GetComponent<ShopButton>();
            if (btn != null)
            {
                btn.Reset();
            }
        }
        foreach (GameObject obj in shipShopUI)
        {
            ShopButton btn = obj.GetComponent<ShopButton>();
            if (btn != null)
            {
                btn.Reset();
            }
        }
    }

    public void exitShop()
    {
        back();
        shop.gameObject.SetActive(false);
    }

    public void shipShop()
    {
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();

        DisableUI(upgradeShopUI);
        EnableUI(shipShopUI);

    }

    public void GivePlayerUnits()
    {
        playerStats.ModifyUnits(1000);
        units = playerStats.GetUnits();
        unitsText.text = units.ToString();
    }

    void EnableUI(List<GameObject> uiList)
    {
        foreach(GameObject obj in uiList)
        {
            ShopButton btn = obj.GetComponent<ShopButton>();
            if(btn != null)
            {
                btn.Enable();
            }
            else
            {
                obj.SetActive(true);
            }
        }
    }

    void DisableUI(List<GameObject> uiList)
    {
        foreach (GameObject obj in uiList)
        {
            obj.SetActive(false);
        }
    }

    public void back()
    {
        shop.gameObject.SetActive(true);
        //shop2.gameObject.SetActive(true);
        DisableUI(shipShopUI);
        EnableUI(upgradeShopUI);
    }

    public void resetPlayerPrefs()
    {
        playerStats.ResetUnits(1000);
        //buy.gameObject.SetActive(true);
        //weaponText.text = "Dual Lasers \n Price: 5 Units";
        //PlayerPrefs.DeleteAll();
    }

    public void ForceDescriptionUpdate(string text)
    {
        descriptionText.text = text;
        itemDescriptions[0].SetActive(true);
    }

    public void UpdateDescriptionDisplay(ShopButton button)
    {
        descriptionText.text = button.upgrade.description;
        itemDescriptions[0].SetActive(true);
    }

    public void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        itemDescriptions[0].SetActive(true);
        Debug.Log("Mouse is over GameObject.");
    }

    public void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        itemDescriptions[0].SetActive(false);
        Debug.Log("Mouse is no longer on GameObject.");
    }

}
