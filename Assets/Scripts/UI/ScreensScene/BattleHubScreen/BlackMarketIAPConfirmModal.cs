using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Scenes;
using Unity.Services.Authentication;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

public class BlackMarketIAPConfirmModal : MonoBehaviour
{
    public Image coinPack;
    public Text title;
    public Text description;
    public Text price;

    public CanvasGroupButton buyBtn, noBtn;
    private IDataProvider _dataProvider;
    private IPrefabFactory _prefabFactory;
    private ISingleSoundPlayer _soundPlayer;

    private IIAPData _currentIAPData;
    public void Initiaize(IDataProvider dataProvider, IPrefabFactory prefabFactory, ISingleSoundPlayer soundPlayer)
    {
        Helper.AssertIsNotNull(dataProvider, prefabFactory, soundPlayer);
        _dataProvider = dataProvider;
        _prefabFactory = prefabFactory;
        _soundPlayer = soundPlayer;

        buyBtn.Initialise(_soundPlayer, Purchase);
        noBtn.Initialise(_soundPlayer, Close);
        title.text = "";
        description.text = "";
        price.text = "";
        coinPack.sprite = null;
    }

    private async void Purchase()
    {
        if(await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
        {
            Close();
            switch (_currentIAPData.IAPNameKeyBase)
            {
                case "Coins100Name":
                    IAPManager.instance.storeController.InitiatePurchase(IAPManager.small_coin_pack);
                    break;
                case "Coins500Name":
                    IAPManager.instance.storeController.InitiatePurchase(IAPManager.medium_coin_pack);
                    break;
                case "Coins1000Name":
                    IAPManager.instance.storeController.InitiatePurchase(IAPManager.large_coin_pack);
                    break;
                case "Coins5000Name":
                    IAPManager.instance.storeController.InitiatePurchase(IAPManager.extralarge_coin_pack);
                    break;
            }
        }
        else
        {
            if(await LandingSceneGod.CheckForInternetConnection())
            {
                MessageBox.Instance.ShowMessage("You have no Internet Connection!");
            }
            else if(AuthenticationService.Instance.IsSignedIn)
            {
                MessageBox.Instance.ShowMessage("You are not signed in.");
            }
        }
    }

    async void OnEnable()
    {
        
        Product product = null;
        string spritePath = "Assets/Resources_moved/Sprites/UI/IAP/";
        switch (_currentIAPData.IAPNameKeyBase)
        {
            case "Coins100Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.small_coin_pack);
                spritePath += "Coins100Pack.png";
                break;
            case "Coins500Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.medium_coin_pack);
                spritePath += "Coins500Pack.png";
                break;
            case "Coins1000Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.large_coin_pack);
                spritePath += "Coins1000Pack.png";
                break;
            case "Coins5000Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.extralarge_coin_pack);
                spritePath += "Coins5000Pack.png";
                break;
        }

        if (product != null)
        {
            price.text = "$ " + product.metadata.localizedPriceString;
            title.text = product.metadata.localizedTitle;
            description.text = product.metadata.localizedDescription;
            SpriteFetcher spriteFetcher = new SpriteFetcher();
            ISpriteWrapper spriteWrapper = await spriteFetcher.GetSpriteAsync(spritePath);
            coinPack.sprite = spriteWrapper.Sprite;
        }
    }

    public void Show(IIAPData iapData)
    {
        Assert.IsNotNull(iapData);
        _currentIAPData = iapData;
        gameObject.SetActive(true);
    }
    private void Close()
    {
        Invoke("HideSelf", 0.25f);
    }

    private void HideSelf()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
