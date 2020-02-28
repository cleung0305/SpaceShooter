using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUV : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Material mat = MR.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.x = transform.position.x;
        mat.mainTextureOffset = offset;
    }
}
