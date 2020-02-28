using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorContainer : MonoBehaviour
{
    [SerializeField] int sectorNumber;
    [SerializeField] GameManager gameManager;
    [SerializeField] List<GameObject> containers;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        if(collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Found player controller");
            gameManager.EnterSector(this);
        }
    }
    
    public int GetSectorNumber()
    {
        return sectorNumber;
    }

    public void Hide()
    {
        foreach(GameObject container in containers)
        {
            container.SetActive(false);
        }
    }

    public void Show()
    {
        foreach (GameObject container in containers)
        {
            container.SetActive(true);
        }
    }
}
