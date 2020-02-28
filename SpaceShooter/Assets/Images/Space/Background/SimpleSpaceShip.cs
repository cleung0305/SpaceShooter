using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpaceShip : MonoBehaviour
{
    // Update is called once per frame
    float speed = 5f;
    void Update()
    {
        transform.Translate(new Vector3(
            Input.GetAxis("Horizontal") * speed,
            Input.GetAxis("Vertical") * speed,
            0)  * Time.deltaTime  ) ;
    }
}
