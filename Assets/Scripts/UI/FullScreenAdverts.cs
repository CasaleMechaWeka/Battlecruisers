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
    public GameObject ThankYouEffect;
    public Button closeButton;


    // Start is called before the first frame update
    void Start()
    {
       gameObject.SetActive(false);
        StartPlatformSpecficAds();


        float xAdjustment = transform.localScale.x;
        float yAdjustment = transform.localScale.y;

        float xad = defaultAd.transform.localScale.x;
        float yad = defaultAd.transform.localScale.y;

        if ((SystemInfo.deviceType == DeviceType.Handheld && DeviceDiagonalSizeInInches() >= 7f))//if tablet
        {
            xAdjustment = 2f;
            yAdjustment = 1f;;
            defaultAd.transform.localScale = new Vector3(xAdjustment, yAdjustment);
        }
        else
        {
            float scaleAdjustment = 100 / (Screen.dpi / 3.2f);
            if (scaleAdjustment > 1f)
            {
                scaleAdjustment = 1f;
            }
            xAdjustment *= scaleAdjustment;
            yAdjustment *= scaleAdjustment;
            transform.localScale = new Vector3(xAdjustment, yAdjustment);
        }

        Button btn = closeButton.GetComponent<Button>();
        btn.onClick.AddListener(closeAdvert);
    }

    // Update is called once per frame
    void Update()
    {
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
        if (gameObject.activeSelf)
        {
            applicationModel.DataProvider.GameModel.PremiumEdition = false;
            if (applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                if (ThankYouEffect != null)
                    ThankYouEffect.SetActive(true);//set based on if the game is premium or not
                closeAdvert();
                return;
            }
            else
            {
                if (ThankYouEffect != null)
                    ThankYouEffect.SetActive(false);//set based on if the game is premium or not
            }
        }
    }

    public void closeAdvert()
    {
        gameObject.SetActive(false);
    }

    public void openAdvert()
    {
        StartPlatformSpecficAds();
    }

    void StartPlatformSpecficAds()
    {
#if FREE_EDITION && UNITY_ANDROID || UNITY_IOS
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
