using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAdvertisingBillboard : MonoBehaviour
{
    public GameObject AdertisingBanner;
    private AdvertisingBannerScrollingText _AdvertisingBannerController;
    public AdvertisingBannerScrollingText AdvertisingBannerController
    { get => _AdvertisingBannerController; }
   
    private bool _LoadAdvert = false;
    public bool LoadAdvert
    {
        get => _LoadAdvert;
        set => _LoadAdvert = value;
    }

    // Start is called before the first frame update
    void Start()
    {
         _AdvertisingBannerController = AdertisingBanner.GetComponent<AdvertisingBannerScrollingText>();
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
