using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    public float blinkTiming = 1.0f;
    public bool fade = true;
    private SpriteRenderer _spriteRenderer;
    private bool transparencySwitch = true;
    private float timeElapsed;
    private float startValue = 0;
    private float endValue = 1;
    private float valueToLerp;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
       timeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (timeElapsed < blinkTiming)
        {
            Color tempcolor = _spriteRenderer.color;
            if (fade)
            {
                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / blinkTiming);
                tempcolor.a = valueToLerp;
            }
            else
            {
                tempcolor.a = endValue;
            }
            
            _spriteRenderer.color = tempcolor;
            timeElapsed += Time.deltaTime;
        }
        else {
            transparencySwitch = !transparencySwitch;
            timeElapsed = 0;
            if (!transparencySwitch)
            {
                startValue = 1;
                endValue = 0;
            }
            else 
            {
                startValue = 0;
                endValue = 1;
            }
        }


    }
}
