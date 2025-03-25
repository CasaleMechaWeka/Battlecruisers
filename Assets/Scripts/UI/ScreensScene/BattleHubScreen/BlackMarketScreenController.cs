using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class BlackMarketScreenController : ScreenController
    {
        public CanvasGroupButton backButton, buyButton;
        private PrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private DataProvider _dataProvider;
        public Transform iapContainer;
        public EventHandler<IAPDataEventArgs> iapDataChanged;
        public GameObject itemPrefab;
        private IAPItemController _currentItem;
        private IIAPData currenIAPData;
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
                    _dataProvider.GameModel.Coins += 275;
                    ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CoinsPack100Purchased"));
                    ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("buy");
                    break;
                case IAPManager.medium_coin_pack:
                    _dataProvider.GameModel.Coins += 900;
                    ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CoinsPack500Purchased"));
                    ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("buy");
                    break;
                case IAPManager.large_coin_pack:
                    _dataProvider.GameModel.Coins += 3750;
                    ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CoinsPack1000Purchased"));
                    ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("buy");
                    break;
                case IAPManager.extralarge_coin_pack:
                    _dataProvider.GameModel.Coins += 20000;
                    ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("CoinsPack5000Purchased"));
                    ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("buy");
                    break;
            }
            _dataProvider.SaveGame();
            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
            try
            {
                bool result = await _dataProvider.SyncCoinsToCloud();
                if (!result)
                    Debug.Log("Sync failed");
            }
            catch
            {
                Debug.Log("Sync failed");
            }
        }

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            PrefabFactory prefabFactory,
            DataProvider dataProvider)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton, confirmModal, screensSceneGod, soundPlayer, prefabFactory, dataProvider, iapContainer);
            Helper.AssertIsNotNull(iapIcon, iapName, iapDescription, iapPrice);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;

            confirmModal.Initiaize(dataProvider, prefabFactory, soundPlayer);
            backButton.Initialise(soundPlayer, GoHome, this);
            buyButton.Initialise(soundPlayer, Buy, this);

            iapDataChanged += IAPDataChangedHandler;
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

            iapIcon.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + args.iapData.IAPIconName + ".png");
            iapName.text = LocTableCache.ScreensSceneTable.GetString(args.iapData.IAPNameKeyBase);
            iapDescription.text = LocTableCache.ScreensSceneTable.GetString(args.iapData.IAPDescriptionKeyBase);
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

            iapPrice.text = product.metadata.localizedPriceString;
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

            byte ii = 0;

            foreach (IAPData iapData in StaticData.IAPs)
            {
                GameObject iapItem = Instantiate(itemPrefab, iapContainer) as GameObject;
                iapItem.GetComponent<IAPItemController>().StaticInitialise(_soundPlayer, iapData, this);
                if (ii == 0)
                {
                    iapItem.GetComponent<IAPItemController>()._clickedFeedback.SetActive(true);
                    _currentItem = iapItem.GetComponent<IAPItemController>();
                    currenIAPData = iapData;
                    iapIcon.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + iapData.IAPIconName + ".png");
                    iapName.text = LocTableCache.ScreensSceneTable.GetString(iapData.IAPNameKeyBase);
                    iapDescription.text = LocTableCache.ScreensSceneTable.GetString(iapData.IAPDescriptionKeyBase);
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
