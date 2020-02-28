using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopButton : MonoBehaviour
{

    [SerializeField] public Upgrade upgrade;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Image icon;
    [SerializeField] TextMeshProUGUI priceTag;
    public int boughtCount = 0;
    public int maxBuyCount = 1;

    private void Awake()
    {
        if (upgrade != null)
        {

            text.text = upgrade.upgradeName;
            if (upgrade.type == 3)
            {
                icon.sprite = upgrade.weapon.projectileImage;
                maxBuyCount = 1;
            }
            else if (upgrade.type == 4)
            {
                icon.sprite = upgrade.ship.baseSprite;
                maxBuyCount = 1;
            }
            else
            {
                maxBuyCount = upgrade.maxBuyCount;
            }

            priceTag.text = upgrade.cost.ToString();
            icon.type = Image.Type.Simple;
            icon.preserveAspect = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Enable()
    {
        if(upgrade != null && boughtCount < maxBuyCount)
        {
            gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        boughtCount = 0;
        Awake();
    }

    public void GetPurchased()
    {
        boughtCount += 1;
        if (boughtCount >= maxBuyCount)
            gameObject.SetActive(false);
    }

    public void ModifyStock(int amount)
    {
        boughtCount = amount;
        if (boughtCount < 0)
            boughtCount = 0;
        if (boughtCount > maxBuyCount)
            boughtCount = maxBuyCount;
    }

}
