using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour {

    public Image blackLayer;
    float alpha = 0;
    float repeatTick = 3;

    public bool fadeOutBool = false;
    public bool fadeInBool = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(fadeOutBool)
        {
            StartCoroutine(performFadeOut());
        }
        else if(fadeInBool)
        {
            StartCoroutine(performFadeIn());
        }
    }

    IEnumerator performFadeOut()
    {
        while (blackLayer.color.a < 1.0f)
        {
            Color c = blackLayer.color;
            c.a += Time.deltaTime / 8;
            blackLayer.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        fadeInBool = true;
        fadeOutBool = false;

        yield return null;
    }

    IEnumerator performFadeIn()
    {
        while (blackLayer.color.a > 0.0f)
        {
            Color c = blackLayer.color;
            c.a -= Time.deltaTime / 8;
            blackLayer.color = c;
            yield return null;
        }
        fadeInBool = false;

        yield return null;
    }
}
