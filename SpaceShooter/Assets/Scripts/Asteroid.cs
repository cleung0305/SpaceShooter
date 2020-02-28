using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] AudioSource destroySound;
    [SerializeField] Collider2D hitBox;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] float growthSpeed = 2f;
    bool waitingForDestroy = false;

    Vector3 growth;
    private void Start()
    {
        growth = new Vector3(growthSpeed, growthSpeed, growthSpeed) * transform.localScale.x;
        health = transform.localScale.x * 100;
    }
    // Update is called once per frame
    void Update()
    {
        if(waitingForDestroy)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
                transform.localScale += new Vector3(growthSpeed, growthSpeed, 0) * Time.deltaTime;
            if (!destroySound.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            animator.SetTrigger("Destroyed");
            hitBox.enabled = false;
            destroySound.Play();
            waitingForDestroy = true; //make sure that the gameobject doesn't destroy itself before the sound is done playing
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShipController ship = collision.gameObject.GetComponent<ShipController>();
        if(ship != null)
        {
            float velocity = collision.relativeVelocity.magnitude;
            if (velocity > 5)
            {
                if(ship.GetId() == 0)
                {
                    ship.TakeDamage(velocity * (velocity / 6), null, false);
                    TakeDamage(velocity * (velocity / 8));
                }
                else
                {
                    ship.TakeDamage(velocity * (velocity / 16), null);
                    TakeDamage(velocity * (velocity / 8));
                }

            }
     
        }
    }
}
