using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxFollow : MonoBehaviour
{
    public float parallax = 2f;
    void Update()
    {
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Material mat = MR.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.x = transform.position.x / transform.localScale.x / parallax;
        offset.y = transform.position.y / transform.localScale.y / parallax;
        mat.mainTextureOffset = offset;
    }
}
