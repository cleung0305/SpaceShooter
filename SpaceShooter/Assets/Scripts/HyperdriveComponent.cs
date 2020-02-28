using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperdriveComponent : MonoBehaviour
{
    GameObject target;
    bool hasTarget = false;
    float speed = 1f;

    [SerializeField] int partNumber;
    private void FixedUpdate()
    {
        if (hasTarget)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            speed += distance * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (distance < .5f)
            {
                target.GetComponent<PlayerStats>().AquireHyperDrivePart(partNumber);
                GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gm.UnlockMusic();
                //gm.ResetPlayerReputation();
                gm.SceneTransition(2, "Center", "inPlay", false);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = collision.gameObject;
            hasTarget = true;
        }
    }
}
