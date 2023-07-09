using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        IDataProvider _dataProvider;

        public CanvasGroupButton backButton,buyButton,blackMarketButton;
        public GameObject PlayerCoinAmount;
        public ShopItemPanel itemPanel;
        public ShopItemDisplayer itemDisplayer;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            _dataProvider = dataProvider;
            //Initialise each button with it's function
            backButton.Initialise(soundPlayer, Home, this);
            buyButton.Initialise(soundPlayer, Buy, this);
            blackMarketButton.Initialise(soundPlayer, BlackMarket, this);
            itemPanel.Initialise(soundPlayer, prefabFactory, dataProvider.GameModel, itemDisplayer);
            itemDisplayer.gameObject.SetActive(false);

/*            Text coins = PlayerCoinAmount.GetComponent<Text>();
            coins.text = (dataProvider.GameModel.Coins).ToString();*/
        }

        public void Update()
        {
            //temp testing purpose
            Text coins = PlayerCoinAmount.GetComponent<Text>();
            if (_dataProvider != null)
            {
                coins.text = (_dataProvider.GameModel.Coins).ToString();
            }

        }


        //All the button fucntions for shop screen
        public void Home()
        {
            _screensSceneGod.GotoHubScreen();
        }
        public void Buy()
        {
           //
        }
        public void BlackMarket()
        {
            _screensSceneGod.GotoBlackMarketScreen();
        }
    }
}