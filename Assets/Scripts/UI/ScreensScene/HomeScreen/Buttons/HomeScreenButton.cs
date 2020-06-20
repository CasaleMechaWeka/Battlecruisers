using BattleCruisers.Data.Models;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : TextButton
    {
        protected IHomeScreen _homeScreen;
        protected IGameModel _gameModel;

        public void Initialise(ISingleSoundPlayer soundPlayer, IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise(soundPlayer, homeScreen);

            Helper.AssertIsNotNull(homeScreen, gameModel);

            _homeScreen = homeScreen;
            _gameModel = gameModel;
        }
    }
}