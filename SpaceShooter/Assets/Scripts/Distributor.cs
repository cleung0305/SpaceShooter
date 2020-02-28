using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//This script is used to randomly distribute objects within the area of the box collider. 
//It can be kinda slow depending on how many objects you want and how far apart you want them to be
public class Distributor : MonoBehaviour
{
    Vector2 BLCorner;
    Vector2 BRCorner;
    Vector2 TRCorner;
    Vector2 TLCorner;
    [SerializeField] BoxCollider2D zone; //area in which to spawn the objects
    [SerializeField] int objectCount = 10; //amount of objects to spawn
    [SerializeField] int minSpacing = 2; //minimum spacing of the objects
    [SerializeField] int maxTries = 1000; //amount of times to try to generate each position 
    [SerializeField] GameObject[] prefabs; //list of the prefabs that can be spawned
    List<Vector2> spawnPositions = new List<Vector2>();

    void Awake()
    {
        //get the corners of the box collider since they will be the boundaries
        BLCorner = new Vector2(-(zone.size.x / 2), -(zone.size.y / 2));
        BRCorner = new Vector2((zone.size.x / 2), -(zone.size.y / 2));
        TLCorner = new Vector2(-(zone.size.x / 2), (zone.size.y / 2));
        TRCorner = new Vector2((zone.size.x / 2), (zone.size.y / 2));

        BLCorner += (Vector2)transform.position;
        BRCorner += (Vector2)transform.position;
        TLCorner += (Vector2)transform.position;
        TRCorner += (Vector2)transform.position;
        GeneratePositions();
    }

    //Generate Vector2s that serve as positions for the objects
    void GeneratePositions()
    {
        for(int i = 0; i < objectCount; i++)
        {
            int j = 0;
            Vector2 newPos = GeneratePosition();
            for(; j < maxTries; j++)
            {
                if (!CheckPositions(newPos))
                {
                    newPos = GeneratePosition();
                }
                else break;//if it can't find a space after the max amount of tries (default is 1000), go to the next object
                
            }
            spawnPositions.Add(newPos);
        }
        SpawnPrefabs();
    }

    //Spawn all of the objects in the generated positions
    void SpawnPrefabs()
    {
        foreach (Vector2 position in spawnPositions)
        {
            //spawn a random prefab from the list at the given position, with a random rotation, and parent it to the Distributor gameObject
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360))), gameObject.transform);
        }
    }

    //Generate a random position within the area of the box collider
    Vector2 GeneratePosition()
    {
        float xPos = Random.Range(BLCorner.x, TRCorner.x);
        float yPos = Random.Range(BLCorner.y, TRCorner.y);
        return new Vector2(xPos, yPos);
    }

    //returns whether or not the position complies with the spacing requirements
    bool CheckPositions(Vector2 newPos)
    {
        foreach(Vector2 position in spawnPositions)
        {
            if(Vector2.Distance(newPos, position) < minSpacing)
            {
                return false;
            }
        }

        return true;
    }
}
