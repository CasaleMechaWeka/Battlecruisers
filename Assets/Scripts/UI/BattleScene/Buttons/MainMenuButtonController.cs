using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class MainMenuButtonController : CanvasGroupButton, IButton
    {
        private IMainMenuManager _mainMenuManager;

        public void Initialise(ISingleSoundPlayer soundPlayer, IMainMenuManager mainMenuManager)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(mainMenuManager);
            _mainMenuManager = mainMenuManager;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _mainMenuManager.ShowMenu();
        }
    }
}