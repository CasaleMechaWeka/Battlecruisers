using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;


namespace BattleCruisers.UI.ScreensScene
{
    public class BlackMarketScreenController : ScreenController
    {
        public CanvasGroupButton backButton, buyButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, buyButton);
            backButton.Initialise(soundPlayer,GoHome, this);
            buyButton.Initialise(soundPlayer, Buy, this);
        }

        public void GoHome()
        {
            _screensSceneGod.GotoShopScreen();
        }

        public void Buy()
        {
            
        }
    }

}
