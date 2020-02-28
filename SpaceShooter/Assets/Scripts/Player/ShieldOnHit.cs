using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOnHit : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
    public void onHit()
    {
        StartCoroutine(effect());
    }
    IEnumerator effect()
    {
        gameObject.GetComponent<Animator>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.GetComponent<Animator>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
    }
}