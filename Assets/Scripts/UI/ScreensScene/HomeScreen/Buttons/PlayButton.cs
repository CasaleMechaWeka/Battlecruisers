using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class PlayButton : HomeScreenButton
    {
        private IGameModel _gameModel;

        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise(homeScreen);

            Assert.IsNotNull(gameModel);
            _gameModel = gameModel;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_gameModel.HasAttemptedTutorial)
            {
                _homeScreen.StartLevel1();
            }
            else
            {
                _homeScreen.StartTutorial();
            }
        }
    }
}