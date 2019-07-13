using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class MainMenuButtonController : CanvasGroupButton
    {
        private IMainMenuManager _mainMenuManager;

        public void Initialise(IMainMenuManager mainMenuManager)
        {
            base.Initialise();

            Assert.IsNotNull(mainMenuManager);
            _mainMenuManager = mainMenuManager;
        }

        protected override void OnClicked()
        {
            _mainMenuManager.ShowMenu();
        }
    }
}