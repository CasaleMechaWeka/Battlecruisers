using BattleCruisers.Data.Models;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : MonoBehaviour, IPointerClickHandler
    {
        protected IHomeScreen _homeScreen;
        protected IGameModel _gameModel;

        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel)
        {
            Helper.AssertIsNotNull(homeScreen, gameModel);

            _homeScreen = homeScreen;
            _gameModel = gameModel;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}