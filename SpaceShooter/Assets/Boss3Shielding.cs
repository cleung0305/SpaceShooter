using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Shielding : WeakPoint
{
    [SerializeField] GameObject weakSpotShipUI;
    [SerializeField] GameObject pivotPoint;
    [SerializeField] GameObject shipSpawnPoint;
    [SerializeField] float targetRot = 90;
    [SerializeField] List<GameObject> spawnableShips = new List<GameObject>();

    bool isDestroyed = false;
    bool isOpen = false;
    bool pivoting = false;
    float closeDelay = 0f;
    float openTime = 0f;
    // Update is called once per frame
    void Update()
    {
        DoUpdateChecks();
        if (shield <= 0)
        {
            shieldRegenRate = 0;
            spriteRenderer.sprite = damagedVersion;
        }
        if(pivoting)
        {
            if(!isOpen)
            {
                pivotPoint.transform.localRotation = Quaternion.Slerp(pivotPoint.transform.localRotation, Quaternion.Euler(0, 0, targetRot), 1 * Time.deltaTime);
                if(targetRot > 0)
                {
                    if (pivotPoint.transform.localEulerAngles.z >= Mathf.Abs(targetRot) - 1)
                    {
                        //Debug.Log(pivotPoint.transform.localEulerAngles.z + " " + Mathf.Abs(targetRot) + " " + targetRot);

                        isOpen = true;
                        SpawnShip();
                    }
                }
                else
                {
                    if (360 - pivotPoint.transform.localEulerAngles.z >= Mathf.Abs(targetRot) - 1)
                    {
                        //Debug.Log(pivotPoint.transform.localEulerAngles.z + " " + Mathf.Abs(targetRot) + " " + targetRot);

                        isOpen = true;
                        SpawnShip();
                    }
                }

            }
            else
            {
                openTime += Time.deltaTime;
                if(openTime >= closeDelay)
                {
                    pivotPoint.transform.localRotation = Quaternion.Slerp(pivotPoint.transform.localRotation, Quaternion.Euler(0, 0, 0), 1 * Time.deltaTime);
                    if (pivotPoint.transform.localEulerAngles.z <= .25f)
                    {
                        float z = pivotPoint.transform.localEulerAngles.z;
                        /*if (targetRot < 0)
                            z = z * -1;*/
                        pivotPoint.transform.Rotate(new Vector3(0, 0, 0-z));
                        pivoting = false;
                        isOpen = false;
                        openTime = 0;
                    }
                }
                
            }
        }
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        spriteRenderer.enabled = false;
        healthBar.enabled = false;
        shieldBar.enabled = false;
        isDestroyed = true;
        weakSpotShipUI.SetActive(true);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public void Open()
    {
        pivoting = true;
    }

    void SpawnShip()
    {
        GameObject spawnedShip = Instantiate(spawnableShips[Random.Range(0, spawnableShips.Count)], shipSpawnPoint.transform.position, Quaternion.identity);
        spawnedShip.GetComponent<Rigidbody2D>().AddForce(spawnedShip.transform.up * 50, ForceMode2D.Impulse);
    }
}
