using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Scenes;
using Unity.Services.Authentication;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Utils.Localisation;

public class BlackMarketIAPConfirmModal : MonoBehaviour
{
    public Image coinPack;
    public Text title;
    public Text description;
    public Text price;

    public CanvasGroupButton buyBtn, noBtn;
    private IDataProvider _dataProvider;
    private PrefabFactory _prefabFactory;
    private ISingleSoundPlayer _soundPlayer;

    private IIAPData _currentIAPData;

    public void Initiaize(IDataProvider dataProvider, PrefabFactory prefabFactory, ISingleSoundPlayer soundPlayer)
    {
        Helper.AssertIsNotNull(dataProvider, prefabFactory, soundPlayer);
        _dataProvider = dataProvider;
        _prefabFactory = prefabFactory;
        _soundPlayer = soundPlayer;

        buyBtn.Initialise(_soundPlayer, Purchase);
        noBtn.Initialise(_soundPlayer, HideSelf);
        title.text = "";
        description.text = "";
        price.text = "";
        coinPack.sprite = null;
    }

    private async void Purchase()
    {
        if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
        {
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
            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
            HideSelf();
        }
        else
        {
            if (await LandingSceneGod.CheckForInternetConnection())
            {
                ScreensSceneGod.Instance.messageBox.ShowMessage("You have no Internet Connection!");
            }
            else if (AuthenticationService.Instance.IsSignedIn)
            {
                ScreensSceneGod.Instance.messageBox.ShowMessage("You are not signed in.");
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
                description.text = LocTableFactory.ScreensSceneTable.GetString("Coins100Description");
                title.text = LocTableFactory.ScreensSceneTable.GetString("Coins100Name");
                break;
            case "Coins500Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.medium_coin_pack);
                spritePath += "Coins500Pack.png";
                description.text = LocTableFactory.ScreensSceneTable.GetString("Coins500Description");
                title.text = LocTableFactory.ScreensSceneTable.GetString("Coins500Name");
                break;
            case "Coins1000Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.large_coin_pack);
                spritePath += "Coins1000Pack.png";
                description.text = LocTableFactory.ScreensSceneTable.GetString("Coins1000Description");
                title.text = LocTableFactory.ScreensSceneTable.GetString("Coins1000Name");
                break;
            case "Coins5000Name":
                product = IAPManager.instance.storeController.products.WithID(IAPManager.extralarge_coin_pack);
                spritePath += "Coins5000Pack.png";
                description.text = LocTableFactory.ScreensSceneTable.GetString("Coins5000Description");
                title.text = LocTableFactory.ScreensSceneTable.GetString("Coins5000Name");
                break;
        }

        if (product != null)
        {
            price.text = product.metadata.localizedPriceString;
#if UNITY_ANDROID || UNITY_EDITOR
            // On Android, the Play store appends the app name in parentheses to the defined name value.
            // For example, if the product name is "1,000 Gems" for an app named "Gem Collector", it will return "1,000 Gems (Gem Collector)".
            // Since the appended app name isn't wanted, trim it off before returning the value.
            int lastParen = title.text.LastIndexOf("(");
            Debug.Log(title.text.LastIndexOf("("));
            if (lastParen > -1)
            {
                title.text = title.text.Substring(0, lastParen - 1).Trim();
            }
#endif

            coinPack.sprite = await SpriteFetcher.GetSpriteAsync(spritePath);
        }
    }

    public void Show(IIAPData iapData)
    {
        Assert.IsNotNull(iapData);
        _currentIAPData = iapData;
        gameObject.SetActive(true);
    }

    private void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
