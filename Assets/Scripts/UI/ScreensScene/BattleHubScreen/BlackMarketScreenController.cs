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
        private ILocTable commonStrings;
        public Image iapIcon;
        public Text iapName;
        public Text iapDescription;
        public Text iapPrice;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton, screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper, iapContainer);
            Helper.AssertIsNotNull(iapIcon, iapName, iapDescription, iapPrice);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;

            backButton.Initialise(soundPlayer, GoHome, this);
            buyButton.Initialise(soundPlayer, Buy, this);

            iapDataChanged += IAPDataChangedHandler;
            commonStrings = LandingSceneGod.Instance.commonStrings;
        }

        private async void IAPDataChangedHandler(object sender, IAPDataEventArgs args)
        {
            _currentItem._clickedFeedback.SetActive(false);
            _currentItem = (IAPItemController)sender;
            ScreensSceneGod.Instance.characterOfBlackmarket.GetComponent<Animator>().SetTrigger("select");

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            ISpriteWrapper spWrapper = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + args.iapData.IAPIconName + ".png");
            iapIcon.sprite = spWrapper.Sprite;
            iapName.text = commonStrings.GetString(args.iapData.IAPNameKeyBase);
            iapDescription.text = commonStrings.GetString(args.iapData.IAPDescriptionKeyBase);
            iapPrice.text = "$" + args.iapData.IAPCost.ToString("#,##0.00");

        }
        public void GoHome()
        {
            _screensSceneGod.GotoShopScreen();
        }

        public void Buy()
        {

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
                    Debug.Log("===> you called me here!!!");
                    iapItem.GetComponent<IAPItemController>()._clickedFeedback.SetActive(true);
                    _currentItem = iapItem.GetComponent<IAPItemController>();

                    SpriteFetcher spriteFetcher = new SpriteFetcher();
                    ISpriteWrapper spWrapper = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + iapData.IAPIconName + ".png");
                    iapIcon.sprite = spWrapper.Sprite;
                    iapName.text = commonStrings.GetString(iapData.IAPNameKeyBase);
                    iapDescription.text = commonStrings.GetString(iapData.IAPDescriptionKeyBase);
                    iapPrice.text = "$" + iapData.IAPCost.ToString("#,##0.00");
                }
                ii++;
            }          
            buyButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            iapDataChanged -= IAPDataChangedHandler;
        }
    }


    public class IAPDataEventArgs : EventArgs
    {
        public IIAPData iapData { get; set; }
    }
}
