using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorLerp : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] float lerpSpeed = .05f;
    Color currentColor;
    float lerpAmt = 0;
    int modifier = 1;
    Color lastColor;
    Color lerpTo;
    private void Awake()
    {
        lastColor = img.color;
        lerpTo = NextColor();
    }
    // Update is called once per frame
    void Update()
    {
        if (lerpSpeed < 1)
            lerpSpeed += .25f * Time.deltaTime;
        lerpAmt += lerpSpeed * modifier * Time.deltaTime;
        if (lerpAmt >= 1)
        {
            lerpAmt = 0;
            lastColor = currentColor;
            lerpTo = NextColor();
        }
        currentColor = Color.Lerp(lastColor, lerpTo, lerpAmt);
        img.color = currentColor;
    }

    Color NextColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }

}
