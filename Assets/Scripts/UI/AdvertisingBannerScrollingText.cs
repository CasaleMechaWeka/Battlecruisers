using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class AdvertisingBannerScrollingText : MonoBehaviour
{
    public bool loadAdvert;
    public GameObject ConfirmationScreen;
    public CanvasGroupButton RemoveAdvertsButton, MerchShoppeButton, PremiumBottomButton, MerchBottomButton;
    public OpenLink merchLink;
    private BoxCollider2D boxCollider;
    private TMP_Text _TextBox;
    private float _xPos;
    private ILocTable _advertisingTable;
    private int _scrollAdjustment;
    private int[] _randomiserArray = new int[16];
    private int _numberOfRandomAttempts = 0;
    private bool _isFirstLoad = true;
    [SerializeField]
    public AudioSource _uiAudioSource;
    private ISingleSoundPlayer _soundPlayer;

    private const string ANIMATOR_TRIGGER = "Play";

    async void Start()
    {
        StartPlatformSpecificAds();

        HideIAPButton();

        _advertisingTable = await LocTableFactory.LoadTableAsync(TableName.ADVERTISING);

        _soundPlayer
                = new SingleSoundPlayer(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource),
                        ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager, 1));

        RemoveAdvertsButton.Initialise(_soundPlayer, ShowPurchaseConfirmationScreenDelayed);
        MerchShoppeButton.Initialise(_soundPlayer, RedirectToMerchShoppe);
        PremiumBottomButton.Initialise(_soundPlayer, ShowPurchaseConfirmationScreenDelayed);
        MerchBottomButton.Initialise(_soundPlayer, RedirectToMerchShoppe);
    }
    public void ShowPurchaseConfirmationScreenDelayed()
    {
        Invoke("ShowPurchaseConfirmationScreen", 0.25f);
    }

    public void RedirectToMerchShoppe()
    {
        Debug.Log("Merch Shoppe Button can be clicked!");
        merchLink.Open();
    }

    public void HideIAPButton()
    {
        RemoveAdvertsButton.gameObject.SetActive(false);
        PremiumBottomButton.gameObject.SetActive(false);
    }

    public void ShowIAPButton()
    {
        RemoveAdvertsButton.gameObject.SetActive(true);
        PremiumBottomButton.gameObject.SetActive(true);
    }

    private void ShowPurchaseConfirmationScreen()
    {
        if (ConfirmationScreen != null)
        {
            ConfirmationScreen.SetActive(true);
        }
    }



    public void stopAdvert()
    {
        gameObject.SetActive(false);

    }

    public void startAdvert()
    {
        StartPlatformSpecificAds();
    }

    void StartPlatformSpecificAds()
    {
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        gameObject.SetActive(false);

        /*#elif UNITY_ANDROID && FREE_EDITION*/
#elif UNITY_ANDROID
        if (!applicationModel.DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
/*#elif UNITY_EDITOR && FREE_EDITION*/
#elif UNITY_EDITOR
        if (!applicationModel.DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
        }
          else
        {
            gameObject.SetActive(false);
            Debug.Log("===> PremiumEdition");
        }
#endif
    }

    void Update()
    {
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;


        if (IAPManager.instance != null)
        {
            if (!applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                ShowIAPButton();
                startAdvert();
            }
            else
            {
                HideIAPButton();
                stopAdvert();
            }
        }

        if (_isFirstLoad)
        {
            if (!loadAdvert)
            {
                return;
            }
            else
            {
                _isFirstLoad = false;
                dummyText();
            }
        }
    }

    public static bool HasParameter(string parameterName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        return false;
    }

    private void clearAdvertShowDummy()
    {
        dummyText();
    }

    private void dummyText()
    {
        setupText();
    }

    private void setupText()
    {
        _xPos = (int)(boxCollider.size.x * 1.8);

        int randomnumber = UnityEngine.Random.Range(1, 16);
        int numberOfRandomAttempts = 0;
        while (numberOfRandomAttempts < 14)
        {
            numberOfRandomAttempts += 1;
            if (_randomiserArray.Contains(randomnumber))
            {
                randomnumber = UnityEngine.Random.Range(1, 8);
            }
            else
            {
                break;
            }
        }

        _numberOfRandomAttempts += 1;

        _randomiserArray[_numberOfRandomAttempts] = randomnumber;
        if (_numberOfRandomAttempts > 13)
        {
            Array.Clear(_randomiserArray, 0, _randomiserArray.Length);
            _numberOfRandomAttempts = 0;
        }

        _TextBox.text = _advertisingTable.GetString("ScrollingAd/" + randomnumber);
        _scrollAdjustment = (int)(_TextBox.text.Length * 13);

    }

    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

}
