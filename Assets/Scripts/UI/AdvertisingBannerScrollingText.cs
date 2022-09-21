using BattleCruisers.Utils.Localisation;
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
    private AdjustAdvertisingBillboard _adjustAdvertisingBillboard;
    private BoxCollider2D boxCollider;
    private TMP_Text _TextBox;
    public float scrollSpeed = 50;
    private float _xPos;
    private ILocTable _advertisingTable;
    private int _scrollAdjustment;
    private int[] _randomiserArray = new int[7];
    private int _numberOfRandomAttempts = 0;
    private bool _ADLoaded = false;
    private bool _isFirstLoad = true;

    //advertising banner
    private BannerView _bannerView;

    // Start is called before the first frame update
    async void Start()
    {
        gameObject.SetActive(false);//default of not active
        #if UNITY_ANDROID
            gameObject.SetActive(true);
#elif UNITY_EDITOR
            gameObject.SetActive(true);
#endif
      //  float heightOfAdvert = getBannerHeight();
        float scaleAdjustment = 50 / getBannerHeight(); 
       // float px_height = heightOfAdvert * (Screen.dpi / 240);
       // float scaleAdjustment = (px_height / heightOfAdvert)* 0.83f;
        //Debug.Log(getBannerHeight());
        Debug.Log(scaleAdjustment);
        float xAdjustment = DefaultBanner.transform.localScale.x * scaleAdjustment;
        float yAdjustment = DefaultBanner.transform.localScale.y * scaleAdjustment;
       // DefaultBanner.transform.localScale = new Vector3(xAdjustment, yAdjustment);
        Debug.Log("helpfulstuff");
        Debug.Log(Screen.dpi);
        Debug.Log(getBannerHeight());

        MobileAds.Initialize(initStatus => { });//initalising Ads as early as possible
        _TextBox = ScrollingTextBox.GetComponent<TMP_Text>();
        boxCollider = TextMask.GetComponent<BoxCollider2D>();
        _advertisingTable = await LocTableFactory.Instance.LoadAdvertisingTableAsync();
        Text textBoxCompanyName = TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = _advertisingTable.GetString("CompanyName");
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
                adUnitId = "ca-app-pub-7490362328602066/6763471166";//test ID>> "ca-app-pub-3940256099942544/6300978111";//only android implementation to start with
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
        ScrollingTextBox.transform.position = new Vector3(_xPos, ScrollingTextBox.transform.position.y, ScrollingTextBox.transform.position.z);
        if (_xPos < -_scrollAdjustment) {
            if (loadAdvert)
            {
                RequestBanner();
            }
        }
    }

    private void setupText() {
        _xPos = (int) (boxCollider.size.x * 1.2);
        ScrollingTextBox.transform.position = new Vector3(_xPos, ScrollingTextBox.transform.position.y, ScrollingTextBox.transform.position.z);

        int randomnumber = UnityEngine.Random.Range(1, 8);
        int numberOfRandomAttempts = 0;
        while (numberOfRandomAttempts < 6) {
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
        if (_numberOfRandomAttempts > 5) {//we have to much history, clear it
            Array.Clear(_randomiserArray, 0, _randomiserArray.Length);
            _numberOfRandomAttempts = 0;
        }

        _TextBox.text = _advertisingTable.GetString("ScrollingAd/" + randomnumber);
        _scrollAdjustment = (int)(_TextBox.text.Length*2.6);

    }

    private void RequestBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();//clear out the old one
            _bannerView = null;
        }

        _ADLoaded = false;

        string adUnitId = "unused";

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-7490362328602066/6763471166";//test ID>> "ca-app-pub-3940256099942544/6300978111";//only android implementation to start with
        #else
            adUnitId = "unexpected_platform";
        #endif

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

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

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("ad loaded");
        _ADLoaded = true;
        setupText();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
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
