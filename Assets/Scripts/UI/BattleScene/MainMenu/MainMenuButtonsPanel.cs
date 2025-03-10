using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class MainMenuButtonsPanel : Panel
    {
        public CanvasGroupButton endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            bool isTutorial,
            IMainMenuManager menuManager)
        {
            Helper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton);
            Helper.AssertIsNotNull(soundPlayer, menuManager);

            endGameButton.Initialise(soundPlayer, menuManager.QuitGame);
            skipTutorialButton.Initialise(soundPlayer, menuManager.QuitGame);
            resumeButton.Initialise(soundPlayer, menuManager.DismissMenu);
            retryButton.Initialise(soundPlayer, menuManager.RetryLevel);
            settingsButton.Initialise(soundPlayer, menuManager.ShowSettings);

            if (isTutorial)
            {
                Destroy(endGameButton.gameObject);
                Destroy(retryButton.gameObject);
            }
            else
            {
                Destroy(skipTutorialButton.gameObject);
            }
        }
    }
}