using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        IDataProvider _dataProvider;

        public CanvasGroupButton backButton, buyButton, blackMarketButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton, blackMarketButton);
            _dataProvider = dataProvider;
            //Initialise each button with its function
            backButton.Initialise(soundPlayer, GoHome, this);
            buyButton.Initialise(soundPlayer, PurchaseCaptainExo, this);
            blackMarketButton.Initialise(soundPlayer, GotoBlackMarket, this);
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
    }
}