using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPMainMenuButtonsPanel : PvPPanel
    {
        public PvPCanvasGroupButton endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton;

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            bool isTutorial,
            IPvPMainMenuManager menuManager)
        {
            PvPHelper.AssertIsNotNull(endGameButton, skipTutorialButton, resumeButton, retryButton, settingsButton);
            PvPHelper.AssertIsNotNull(soundPlayer, menuManager);

            endGameButton.Initialise(soundPlayer, menuManager.QuitGame);
            skipTutorialButton.Initialise(soundPlayer, menuManager.QuitGame);
            resumeButton.Initialise(soundPlayer, menuManager.DismissMenu);
            retryButton.Initialise(soundPlayer, menuManager.RetryLevel);
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