using BattleCruisers.Data.Models;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class HomeScreenButton : TextButton
    {
        protected IHomeScreen _homeScreen;
        protected GameModel _gameModel;
        [SerializeField]
        private UnityEvent clickAction;

        public virtual void Initialise(SingleSoundPlayer soundPlayer, IHomeScreen homeScreen, GameModel gameModel)
        {
            base.Initialise(soundPlayer, homeScreen);

            Helper.AssertIsNotNull(homeScreen, gameModel); //stop application from running if these are not set already

            _homeScreen = homeScreen;
            _gameModel = gameModel;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            clickAction.Invoke();
        }
    }
}