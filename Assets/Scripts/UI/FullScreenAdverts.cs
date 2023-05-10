using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
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
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FullScreenAdverts : MonoBehaviour
{
    public DefaultAdvertController defaultAd;
    public Button closeButton;

    private ISettingsManager settingsManager; // For fullscreen ads on premium :)


    // Start is called before the first frame update
    void Start()
    {
        

        StartPlatformSpecficAds();
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
        defaultAd.UpdateImage();
        StartPlatformSpecficAds();
    }

    void StartPlatformSpecficAds()
    {
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
        settingsManager = applicationModel.DataProvider.SettingsManager;

#if FREE_EDITION && (UNITY_ANDROID || UNITY_IOS)
        if (!applicationModel.DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
        }
        else
        {
            Assert.IsNotNull(settingsManager);
            // For premium users, show ads only if ShowAds setting is enabled
            gameObject.SetActive(settingsManager.ShowAds);
        }
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
