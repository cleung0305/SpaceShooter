using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour
{
    [SerializeField] GameObject shop;
    [SerializeField] GameObject playerUI;
    [SerializeField] ShopControl shopControl;

    [SerializeField] Vector2 repRange;
    bool canAppear = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetComponent<PlayerStats>().reputation >= repRange.x && collision.GetComponent<PlayerStats>().reputation < repRange.y)
        {
            shopControl.SetPlayerStats(collision.GetComponent<PlayerStats>());
            shop.gameObject.SetActive(true);
            canAppear = false;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shop.gameObject.SetActive(false);
            canAppear = true;
        }
    }

}
