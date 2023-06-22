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

        public CanvasGroupButton backButton,buyButton;
        public GameObject PlayerCoinAmount;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            _dataProvider = dataProvider;
            backButton.Initialise(soundPlayer, Home, this);

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

        public void Home()
        {
            _screensSceneGod.GotoHubScreen();
        }
        public void Buy()
        {
           //
        }
    }
}