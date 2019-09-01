using BattleCruisers.Data.Models;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : TextButton
    {
        protected IHomeScreen _homeScreen;
        protected IGameModel _gameModel;

        public void Initialise(ISoundPlayer soundPlayer, IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(homeScreen, gameModel);

            _homeScreen = homeScreen;
            _gameModel = gameModel;
        }
    }
}