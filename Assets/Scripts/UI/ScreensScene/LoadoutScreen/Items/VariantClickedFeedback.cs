using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantClickedFeedback : MonoBehaviour
{
    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    public bool IsVisible
    {
        set
        {
            if (value)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            }
            else
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 64f/255);
            }
        }
    }
}
