using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FullScreenAdverts : MonoBehaviour
{
    public DefaultAdvertController defaultAd;
    public Button closeButton;
    private SettingsManager settingsManager; // For fullscreen ads on premium :)

    // Start is called before the first frame update
    void Start()
    {
#if THIRD_PARTY_PUBLISHER
        StartPlatformSpecficAds();
#endif
        Button btn = closeButton.GetComponent<Button>();
        btn.onClick.AddListener(CloseAdvert);
    }
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
        settingsManager = DataProvider.SettingsManager;
#if THIRD_PARTY_PUBLISHER
        gameObject.SetActive(false);
#endif
        // #if FREE_EDITION && (UNITY_ANDROID || UNITY_IOS)
#if UNITY_ANDROID || UNITY_IOS
        if (!DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
            LandingSceneGod.MusicPlayer.PlayAdsMusic();
        }
        else
        {
            Assert.IsNotNull(settingsManager);
            // For premium users, show ads only if ShowAds setting is enabled
            gameObject.SetActive(settingsManager.ShowAds);
        }
// #elif UNITY_EDITOR && FREE_EDITION
#elif UNITY_EDITOR
        if (!DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
            LandingSceneGod.MusicPlayer.PlayAdsMusic();
        }
        else
        {
            Assert.IsNotNull(settingsManager);
            // For premium users, show ads only if ShowAds setting is enabled
            gameObject.SetActive(settingsManager.ShowAds);
        }
#else
        Assert.IsNotNull(settingsManager);
        gameObject.SetActive(settingsManager.ShowAds);
    //gameObject.SetActive(false);
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
