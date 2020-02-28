using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] int xBound;
    [SerializeField] int yBound;


    int dir = 0;

    [SerializeField] float moveAmount = 0;
    Vector3 movement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        GetNextMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if(dir == 0)
        {
            if(moveAmount > xBound)
            {
                GetNextDir();
            }
        }
        else if (dir == 1)
        {
            if (moveAmount > yBound)
            {
                GetNextDir();
            }
        }
        else if (dir == 2)
        {
            if (moveAmount > xBound)
            {
                GetNextDir();
            }
        }
        else if (dir == 3)
        {
            if (moveAmount > yBound)
            {
                GetNextDir();
            }
        }
        moveAmount += speed * Time.deltaTime;
        gameObject.transform.position += movement * speed * Time.deltaTime;
    }

    void GetNextDir()
    {
        Debug.Log("next");
        dir += 1;
        if(dir > 3)
        {
            dir = 0;
        }
        moveAmount = 0;
        GetNextMovement();
    }

    void GetNextMovement()
    {
        switch (dir)
        {
            case 0:
                movement.x = 1;
                movement.y = 0;
                break;
            case 1:
                movement.x = 0;
                movement.y = 1;
                break;
            case 2:
                movement.x = -1;
                movement.y = 0;
                break;
            case 3:
                movement.x = 0;
                movement.y = -1;
                break;
        }
    }
}
