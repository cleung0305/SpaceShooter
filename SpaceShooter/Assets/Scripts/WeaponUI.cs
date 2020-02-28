using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponUI : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Image childImage;
    [SerializeField] float fadeSpeed = 1;
    [SerializeField] float fadeDelay = 1;

    float noFadeTime = 0f;
    float opacity = 0f;
    float tempFadeSpeed;
    bool useTempSpeed = false;
    Color opacityColor;
    // Start is called before the first frame update
    void Start()
    {
        opacityColor = img.color;
        opacity = opacityColor.a;
    }

    public void SlotImage(Sprite sprite)
    {
        childImage.sprite = sprite;
        childImage.type = Image.Type.Simple;
        childImage.preserveAspect = true;
    }

    public void GetSelected()
    {
        opacity = 1;
        opacityColor.a = 1f;
        noFadeTime = fadeDelay;
        useTempSpeed = false;
    }

    public void ImmediateFade()
    {
        opacity = 0;
    }

    public void AccelerateFade()
    {
        noFadeTime = 0f;
        tempFadeSpeed = 2f;
        useTempSpeed = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        noFadeTime -= 1 * Time.deltaTime;
        if (noFadeTime <= 0)
        {
            if(useTempSpeed)
                opacity -= 1 * tempFadeSpeed * Time.deltaTime;
            else
                opacity -= 1 * fadeSpeed * Time.deltaTime;
            opacity = opacity < 0 ? 0 : opacity;
        }
        opacityColor.a = opacity;
        img.color = opacityColor;
        childImage.color = opacityColor;
    }
}
