using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class MainMenuButtonController : CanvasGroupButton
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