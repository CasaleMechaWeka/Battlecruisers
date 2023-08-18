using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class BlackMarketScreenController : ScreenController
    {
        public CanvasGroupButton backButton, buyButton;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public Transform iapContainer;
        public EventHandler<IAPDataEventArgs> iapDataChanged;
        public GameObject itemPrefab;
        private IAPItemController _currentItem;
        private IIAPData currenIAPData;
        private ILocTable commonStrings;
        public Image iapIcon;
        public Text iapName;
        public Text iapDescription;
        public Text iapPrice;
        public BlackMarketIAPConfirmModal confirmModal;

        public EventHandler<IAPEventArgs> purchasedIAP;

        public static BlackMarketScreenController Instance;

        private async void PurchasedIAP(object sender, IAPEventArgs e)
        {
            switch (e.CoinsPack)
            {
                case IAPManager.small_coin_pack:
                    Debug.Log("===>" + IAPManager.small_coin_pack);
                    break;
                case IAPManager.medium_coin_pack:
                    Debug.Log("===>" + IAPManager.medium_coin_pack);
                    break;
                case IAPManager.large_coin_pack:
                    Debug.Log("===>" + IAPManager.large_coin_pack);
                    break;
                case IAPManager.extralarge_coin_pack:
                    Debug.Log("===>" + IAPManager.extralarge_coin_pack);
                    break;
            }
        }

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton, confirmModal, screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper, iapContainer);
            Helper.AssertIsNotNull(iapIcon, iapName, iapDescription, iapPrice);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;

            confirmModal.Initiaize(dataProvider, prefabFactory, soundPlayer);
            backButton.Initialise(soundPlayer, GoHome, this);
            buyButton.Initialise(soundPlayer, Buy, this);

            iapDataChanged += IAPDataChangedHandler;
            commonStrings = LandingSceneGod.Instance.commonStrings;
        }
        private void Start()
        {
            purchasedIAP += PurchasedIAP;
            if (Instance == null)
                Instance = this;
        }

        private async void IAPDataChangedHandler(object sender, IAPDataEventArgs args)
        {
            _currentItem._clickedFeedback.SetActive(false);
            _currentItem = (IAPItemController)sender;
            currenIAPData = args.iapData;
            ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("select");

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            ISpriteWrapper spWrapper = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + args.iapData.IAPIconName + ".png");
            iapIcon.sprite = spWrapper.Sprite;
            iapName.text = commonStrings.GetString(args.iapData.IAPNameKeyBase);
            iapDescription.text = commonStrings.GetString(args.iapData.IAPDescriptionKeyBase);
            DisplayPrice();
        }

        private void DisplayPrice()
        {
            Product product = null;

            switch (currenIAPData.IAPNameKeyBase)
            {
                case "Coins100Name":
                    product = IAPManager.instance.storeController.products.WithID(IAPManager.small_coin_pack);
                    break;
                case "Coins500Name":
                    product = IAPManager.instance.storeController.products.WithID(IAPManager.medium_coin_pack);
                    break;
                case "Coins1000Name":
                    product = IAPManager.instance.storeController.products.WithID(IAPManager.large_coin_pack);
                    break;
                case "Coins5000Name":
                    product = IAPManager.instance.storeController.products.WithID(IAPManager.extralarge_coin_pack);
                    break;
            }

            iapPrice.text = "$ " + product.metadata.localizedPriceString;
        }
        public void GoHome()
        {
            _screensSceneGod.GotoShopScreen();
        }

        public void Buy()
        {
            confirmModal.Show(currenIAPData);
        }

        public async void InitialiseIAPs()
        {
            // remove all old children to refresh
            IAPItemController[] items = iapContainer.gameObject.GetComponentsInChildren<IAPItemController>();
            foreach (IAPItemController item in items)
            {
                DestroyImmediate(item.gameObject);
            }
            buyButton.gameObject.SetActive(false);

            await Task.Delay(100);

            byte ii = 0;

            foreach (IAPData iapData in _dataProvider.GameModel.IAPs)
            {
                GameObject iapItem = Instantiate(itemPrefab, iapContainer) as GameObject;
                iapItem.GetComponent<IAPItemController>().StaticInitialise(_soundPlayer, iapData, this);
                if (ii == 0)
                {
                    iapItem.GetComponent<IAPItemController>()._clickedFeedback.SetActive(true);
                    _currentItem = iapItem.GetComponent<IAPItemController>();
                    currenIAPData = iapData;
                    SpriteFetcher spriteFetcher = new SpriteFetcher();
                    ISpriteWrapper spWrapper = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + iapData.IAPIconName + ".png");
                    iapIcon.sprite = spWrapper.Sprite;
                    iapName.text = commonStrings.GetString(iapData.IAPNameKeyBase);
                    iapDescription.text = commonStrings.GetString(iapData.IAPDescriptionKeyBase);
                    DisplayPrice();
                }
                ii++;
            }
            buyButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            iapDataChanged -= IAPDataChangedHandler;
            purchasedIAP -= PurchasedIAP;
        }
    }


    public class IAPDataEventArgs : EventArgs
    {
        public IIAPData iapData { get; set; }
    }
    public class IAPEventArgs : EventArgs
    {
        public string CoinsPack;
    }
}
