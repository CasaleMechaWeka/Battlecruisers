using BattleCruisers.Data.Models;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : ClickableTogglable
    {
        protected IHomeScreen _homeScreen;
        protected IGameModel _gameModel;

        private MaskableGraphic _text;
        protected override MaskableGraphic Graphic => _text;

        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise();

            Helper.AssertIsNotNull(homeScreen, gameModel);

            _homeScreen = homeScreen;
            _gameModel = gameModel;

            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);
        }
    }
}