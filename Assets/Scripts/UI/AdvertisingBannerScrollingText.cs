using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using GoogleMobileAds.Api;
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
    //advertising banner
    private BannerView _bannerView;

    // Start is called before the first frame update
    async void Start()
    {
gameObject.SetActive(false);//default of not active
#if UNITY_ANDROID && FREE_EDITION
            gameObject.SetActive(true);
#elif UNITY_EDITOR && FREE_EDITION
        gameObject.SetActive(true);
#endif
        HideIAPButton();

        float xAdjustment = transform.localScale.x;
        float yAdjustment = transform.localScale.y;

        if ((SystemInfo.deviceType == DeviceType.Handheld && DeviceDiagonalSizeInInches() >= 7f /*6.5f*/))//if tablet
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

        

        MobileAds.Initialize(initStatus => { });//initalising Ads as early as possible
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

    public void HideIAPButton() {
        RemoveAdvertsButton.gameObject.SetActive(false);
    }

    public void ShowIAPButton() {
        RemoveAdvertsButton.gameObject.SetActive(true);
    }

    private void ShowPurchaseConfirmationScreen() {
        if (ConfirmationScreen != null) {
            ConfirmationScreen.SetActive(true);
        }
    }

    private float getBannerHeight() {
        if (_bannerView != null)
        {
            _bannerView.Destroy();//clear out the old one
            _bannerView = null;
        }

        _ADLoaded = false;

        string adUnitId = "unused";

        #if UNITY_ANDROID
                adUnitId = "ca-app-pub-7490362328602066/4510644663";//test ID>> "ca-app-pub-3940256099942544/6300978111";//only android implementation to start with
        #else
                    adUnitId = "unexpected_platform";
        #endif

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        return _bannerView.GetHeightInPixels();
    }

    public void stopAdvert() {
        gameObject.SetActive(false);
        if (_bannerView != null)
        {
            _bannerView.Hide();
            _bannerView.Destroy();//clear out the old one
            _bannerView = null;
        }
    }

    public void startAdvert() {
        loadAdvert = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
        if (gameObject.activeSelf)
        {
            if (applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                if (ThankYouEffect != null)
                    ThankYouEffect.SetActive(true);//set based on if the game is premium or not
                stopAdvert();
            }
            else {
                if (ThankYouEffect != null)
                    ThankYouEffect.SetActive(false);//set based on if the game is premium or not
            }
            return;
        }
      

        if (IAPManager.instance != null) {
            ShowIAPButton();
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
                RequestBanner();//first request for banner
            }
        }

        if (!_ADLoaded)
        {
            return;
        }

        if (_bannerView == null) {
            RequestBanner();
        }

        _xPos -= Time.deltaTime * scrollSpeed;
        ScrollingTextBox.transform.localPosition = new Vector3(_xPos, ScrollingTextBox.transform.localPosition.y, ScrollingTextBox.transform.localPosition.z);
        if (_xPos < -_scrollAdjustment) {
            if (loadAdvert)
            {
                clearAdvertShowDummy();
                Invoke("RequestBanner", 4.5f);
            }
        }
    }

    private void clearAdvertShowDummy() {
       // _ADLoaded = false;
        DefaultBanner.SetActive(true);
        dummyText();
        _bannerView.Hide();
    }

    private void dummyText() {
        _xPos = (int)(boxCollider.size.x * 1.8);
        ScrollingTextBox.transform.localPosition = new Vector3(_xPos, ScrollingTextBox.transform.localPosition.y, ScrollingTextBox.transform.localPosition.z);
        _TextBox.text = "*** VOTE PRESIDENTRON *** VOTE PRESIDENTRON *** VOTE PRESIDENTRON ***  VOTE PRESIDENTRON ***";
    }

    private void setupText() {

        _xPos = (int) (boxCollider.size.x * 1.8);

        ScrollingTextBox.transform.localPosition = new Vector3(_xPos, ScrollingTextBox.transform.localPosition.y, ScrollingTextBox.transform.localPosition.z);

        int randomnumber = UnityEngine.Random.Range(1, 16);
        int numberOfRandomAttempts = 0;
        while (numberOfRandomAttempts < 14) {
            numberOfRandomAttempts += 1;
            if (_randomiserArray.Contains(randomnumber))
            {
                randomnumber = UnityEngine.Random.Range(1, 8);
            }
            else {
                break;
            }
        }

        _numberOfRandomAttempts += 1;

        _randomiserArray[_numberOfRandomAttempts] = randomnumber;
        if (_numberOfRandomAttempts > 13) {//we have to much history, clear it
            Array.Clear(_randomiserArray, 0, _randomiserArray.Length);
            _numberOfRandomAttempts = 0;
        }

        _TextBox.text = _advertisingTable.GetString("ScrollingAd/" + randomnumber);
        _scrollAdjustment = (int)(_TextBox.text.Length*13);

    }

    private void RequestBanner()
    {
        DefaultBanner.SetActive(true);

        if (_bannerView != null)
        {
            _bannerView.Destroy();//clear out the old one
            _bannerView = null;
        }

        string adUnitId = "unused";

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-7490362328602066/6763471166";//test ID>> "ca-app-pub-3940256099942544/6300978111";//only android implementation to start with
#else
            adUnitId = "unexpected_platform";
#endif
        if ((SystemInfo.deviceType == DeviceType.Handheld && DeviceDiagonalSizeInInches() >= 7f /*6.5f*/))
        {
            _bannerView = new BannerView(adUnitId, AdSize.IABBanner, AdPosition.Bottom);
        } else {
            _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        }

        // Called when an ad request has successfully loaded.
        _bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        _bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        _bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        _bannerView.OnAdClosed += HandleOnAdClosed;

        // Load the banner with the request.
        _bannerView.LoadAd(CreateAdRequest());
    }

    public AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
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

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DefaultBanner.SetActive(true);
        _ADLoaded = true;
        setupText();//carry on anyway - we have a dummy add to serve up
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
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
