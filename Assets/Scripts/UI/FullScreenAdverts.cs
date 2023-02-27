using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
//using GoogleMobileAds.Api;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenAdverts : MonoBehaviour
{
    public GameObject defaultAd;
    public Button closeButton;


    // Start is called before the first frame update
    void Start()
    {
       //gameObject.SetActive(false);
        StartPlatformSpecficAds();


        float xAdjustment = transform.localScale.x;
        float yAdjustment = transform.localScale.y;


        if ((SystemInfo.deviceType == DeviceType.Handheld && DeviceDiagonalSizeInInches() >= 7f))//if tablet
        {
            xAdjustment = 1f;
            yAdjustment = 1f;;
            defaultAd.transform.localScale = new Vector3(xAdjustment, yAdjustment);
        }
        else
        {
            float scaleAdjustment = 100 / (Screen.dpi / 3.2f);
            //if (scaleAdjustment > 1f)
            //{
                scaleAdjustment = 1f;
            //}
            xAdjustment *= scaleAdjustment;
            yAdjustment *= scaleAdjustment;
            transform.localScale = new Vector3(xAdjustment, yAdjustment);
        }

        Button btn = closeButton.GetComponent<Button>();
        btn.onClick.AddListener(CloseAdvert);
    }

    // Update is called once per frame


    public void CloseAdvert()
    {
        gameObject.SetActive(false);
    }

    public void OpenAdvert()
    {
        StartPlatformSpecficAds();
    }

    void StartPlatformSpecficAds()
    {
#if FREE_EDITION && (UNITY_ANDROID || UNITY_IOS)
        gameObject.SetActive(true);
#elif UNITY_EDITOR && FREE_EDITION
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }

    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }

}
