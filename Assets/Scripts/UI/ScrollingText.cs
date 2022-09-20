using BattleCruisers.Utils.Localisation;
using GoogleMobileAds.Api;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public GameObject TextMask;
    public GameObject TextCompanyName;
    private BoxCollider2D boxCollider;
    private TMP_Text _TextBox;
    public float scrollSpeed = 50;
    private float _xPos;
    private ILocTable _advertisingTable;
    private int _scrollAdjustment;
    private int[] _randomiserArray = new int[7];
    private int _numberOfRandomAttempts = 0;
    private bool _ADLoaded = false;

    //advertising banner
    private BannerView _bannerView;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });//initalising Ads as early as possible
        _TextBox = GetComponent<TMP_Text>();
        boxCollider = TextMask.GetComponent<BoxCollider2D>();
        Text textBoxCompanyName = TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = "";
        RequestBanner();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ADLoaded)
        {
            return;
        }
        _xPos -= Time.deltaTime * scrollSpeed;
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);
        if (_xPos < -_scrollAdjustment) {
            RequestBanner();

        }
    }

    private async void setupText() {
        _xPos = (int) (boxCollider.size.x * 1.2);
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);

        _advertisingTable = await LocTableFactory.Instance.LoadAdvertisingTableAsync();
        Text textBoxCompanyName = TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = _advertisingTable.GetString("CompanyName");
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
        }

        _ADLoaded = false;

        string adUnitId = "unused";

        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/6300978111";//only android implementation to start with
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
