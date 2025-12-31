using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPMainMenuButtonsPanel : Panel
    {
        public PvPCanvasGroupButton endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton;

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            bool isTutorial,
            IMainMenuManager menuManager)
        {
            PvPHelper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton);
            PvPHelper.AssertIsNotNull(soundPlayer, menuManager);

            endGameButton.Initialise(soundPlayer, menuManager.QuitGame);
            //    skipTutorialButton.Initialise(soundPlayer, menuManager.QuitGame);
            resumeButton.Initialise(soundPlayer, menuManager.DismissMenu);
            //    retryButton.Initialise(soundPlayer, menuManager.RetryLevel);
            settingsButton.Initialise(soundPlayer, menuManager.ShowSettings);

            /*            if (isTutorial)
                        {
                            Destroy(endGameButton.gameObject);
                            Destroy(retryButton.gameObject);
                        }
                        else
                        {
                            Destroy(skipTutorialButton.gameObject);
                        }*/

            // pvp
            Destroy(skipTutorialButton.gameObject);
            Destroy(retryButton.gameObject);
        }
    }
}