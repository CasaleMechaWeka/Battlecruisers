using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        IDataProvider _dataProvider;

        public CanvasGroupButton backButton, buyButton, blackMarketButton;
        public Transform itemContainer;
        public GameObject itemPrefab;
        public CaptainsContainer captainsContainer;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton, blackMarketButton, captainsContainer);
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            //Initialise each button with its function
            backButton.Initialise(_soundPlayer, GoHome, this);
            buyButton.Initialise(_soundPlayer, PurchaseCaptainExo, this);
            blackMarketButton.Initialise(_soundPlayer, GotoBlackMarket, this);
            captainsContainer.Initialize();
        }



        //All the button fucntions for shop screen
        public void GoHome()
        {
            _screensSceneGod.GotoHubScreen();
        }
        public void PurchaseCaptainExo()
        {

        }

        public void GotoBlackMarket()
        {
            _screensSceneGod.GotoBlackMarketScreen();
        }

        public async void InitiaiseShop()
        {
            await Task.Delay(100);
            foreach (int index in _dataProvider.GameModel.CaptainExoList)
            {
                GameObject captainItem = Instantiate(itemPrefab, itemContainer) as GameObject;
                CaptainExo captainExo = Instantiate(_prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.AllKeys[index]));
                captainItem.GetComponent<CaptainItemController>().StaticInitialise(_soundPlayer, captainExo.CaptainExoImage, _dataProvider.GameModel.Captains[index], captainsContainer);
            }

        }
    }
}