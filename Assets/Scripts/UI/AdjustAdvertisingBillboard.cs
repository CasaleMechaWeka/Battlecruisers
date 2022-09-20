using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAdvertisingBillboard : MonoBehaviour
{

    private bool _LoadAdvert = false;
    public bool LoadAdvert
    {
        get => _LoadAdvert;
        set => _LoadAdvert = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);//default of not active

        #if UNITY_ANDROID
            gameObject.SetActive(true);
        #elif UNITY_EDITOR
                    gameObject.SetActive(true);
        #endif

        if (gameObject.activeSelf) //if active we might want to do some things to it
        { 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetBannerHeight()
    {
        return Mathf.RoundToInt(50 * Screen.dpi / 160);
    }

    public float GetBannerWidth()
    {
        return Mathf.RoundToInt(320 * Screen.dpi / 160);
    }
}
