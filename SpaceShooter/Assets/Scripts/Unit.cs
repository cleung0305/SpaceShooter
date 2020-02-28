using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    GameObject target;
    bool hasTarget = false;

    float speed = 1f;
    private void FixedUpdate()
    {
        if(hasTarget)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            speed += distance * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if(distance < .5f)
            {
                target.GetComponent<PlayerStats>().ModifyUnits(10);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            target = collision.gameObject;
            hasTarget = true;
        }
    }
}
