using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasClamp : MonoBehaviour
{
    [SerializeField] Vector3 initialPos;
    [SerializeField] Quaternion initialRot;
    // Update is called once per frame
    private void Awake()
    {
        initialPos = transform.localPosition;
        initialRot = transform.rotation;
    }
    void Update()
    {
        transform.rotation = initialRot;
        transform.localPosition = initialPos;
    }
    private void FixedUpdate()
    {
        transform.rotation = initialRot;
        transform.localPosition = initialPos;
    }
}
