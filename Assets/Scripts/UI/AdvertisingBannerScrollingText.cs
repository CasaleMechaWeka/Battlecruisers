using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisingBannerScrollingText : MonoBehaviour
{
    public bool loadAdvert;
    public GameObject TextMask;
    public GameObject TextCompanyName;
    public GameObject ScrollingTextBox;
    public GameObject MainBannerFront;
    public GameObject DefaultBanner;
    public GameObject ConfirmationScreen;
    public CanvasGroupButton RemoveAdvertsButton;
    public GameObject ThankYouEffect;
    public Animator ThankYouAnimator;
    private BoxCollider2D boxCollider;
    private TMP_Text _TextBox;
    public float scrollSpeed = 50;
    private float _xPos;
    private ILocTable _advertisingTable;
    private int _scrollAdjustment;
    private int[] _randomiserArray = new int[16];
    private int _numberOfRandomAttempts = 0;
    private bool _ADLoaded = false;
    private bool _isFirstLoad = true;
    [SerializeField]
    public AudioSource _uiAudioSource;
    private ISingleSoundPlayer _soundPlayer;

    private const string ANIMATOR_TRIGGER = "Play";

    async void Start()
    {
        StartPlatformSpecificAds();

        HideIAPButton();

        float xAdjustment = transform.localScale.x;
        float yAdjustment = transform.localScale.y;

        if ((SystemInfo.deviceType == DeviceType.Handheld && DeviceDiagonalSizeInInches() >= 7f))
        {
            xAdjustment = 1.25f;
            yAdjustment = 1.25f;
            transform.localScale = new Vector3(xAdjustment, yAdjustment);
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


        _TextBox = ScrollingTextBox.GetComponent<TMP_Text>();
        boxCollider = TextMask.GetComponent<BoxCollider2D>();
        _advertisingTable = await LocTableFactory.Instance.LoadAdvertisingTableAsync();
        Text textBoxCompanyName = TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = _advertisingTable.GetString("CompanyName");

        _soundPlayer
                = new SingleSoundPlayer(
                    new SoundFetcher(),
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource),
                        ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager, 1));
        RemoveAdvertsButton.Initialise(_soundPlayer, ShowPurchaseConfirmationScreenDelayed);

    }
    private void ShowPurchaseConfirmationScreenDelayed()
    {
        Invoke("ShowPurchaseConfirmationScreen", 0.25f);
    }

    public void HideIAPButton()
    {
        RemoveAdvertsButton.gameObject.SetActive(false);
    }

    public void ShowIAPButton()
    {
        RemoveAdvertsButton.gameObject.SetActive(true);
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
#elif UNITY_ANDROID && FREE_EDITION
        if (!applicationModel.DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
#elif UNITY_EDITOR && FREE_EDITION
         if (!applicationModel.DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
        }
          else
        {
            gameObject.SetActive(false);
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
                PlayThankYouAnimation();
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

        _xPos -= Time.deltaTime * scrollSpeed;
        ScrollingTextBox.transform.localPosition = new Vector3(_xPos, ScrollingTextBox.transform.localPosition.y, ScrollingTextBox.transform.localPosition.z);

        if (_xPos < -_scrollAdjustment)
        {
            if (loadAdvert)
            {
                Invoke("dummyText", 1.0f);
            }
        }
    }

    private void PlayThankYouAnimation()
    {
        ThankYouAnimator.SetTrigger(ANIMATOR_TRIGGER);
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
        DefaultBanner.SetActive(true);
        dummyText();
    }

    private void dummyText()
    {
        setupText();
    }

    private void setupText()
    {

        _xPos = (int)(boxCollider.size.x * 1.8);

        ScrollingTextBox.transform.localPosition = new Vector3(_xPos, ScrollingTextBox.transform.localPosition.y, ScrollingTextBox.transform.localPosition.z);

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

    private void RequestBanner()
    {
        DefaultBanner.SetActive(true);
    }

    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        DefaultBanner.SetActive(false);
        _ADLoaded = true;
        setupText();
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
