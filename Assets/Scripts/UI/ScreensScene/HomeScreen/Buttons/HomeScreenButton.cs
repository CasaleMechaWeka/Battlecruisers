using BattleCruisers.Data.Models;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : TextButton
    {
        protected IHomeScreen _homeScreen;
        protected IGameModel _gameModel;

        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise();

            Helper.AssertIsNotNull(homeScreen, gameModel);

            _homeScreen = homeScreen;
            _gameModel = gameModel;
        }
    }
}