using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Material mat = MR.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.x += Time.deltaTime / 10f;
        mat.mainTextureOffset = offset;
    }
}
