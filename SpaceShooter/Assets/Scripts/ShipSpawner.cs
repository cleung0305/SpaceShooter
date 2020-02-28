using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script simply spawns ships
//It works pretty similarly to the Distributor, but it will likely have many different functions in the future
public class ShipSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 5;
    [SerializeField] int maxShips = 10;
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] List<GameObject> ships;

    float timeSinceLastSpawn = 0f;
    int spawnedShips = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= spawnRate && spawnedShips < maxShips)
        {
            SpawnShip();
        }
    }

    public void ReduceSpawnedAmount(int amount)
    {
        spawnedShips -= amount;
    }

    void SpawnShip()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += Random.Range(-(spawnArea.size.x / 2), spawnArea.size.x / 2);
        spawnPosition.y += Random.Range(-(spawnArea.size.y / 2), spawnArea.size.y / 2);
        NpcController spawnedShip = Instantiate(ships[Random.Range(0, ships.Count)], spawnPosition, Quaternion.identity).GetComponent<NpcController>();
        spawnedShip.SetSpawner(this);
        timeSinceLastSpawn = 0f;
        spawnedShips += 1;
    }
}
