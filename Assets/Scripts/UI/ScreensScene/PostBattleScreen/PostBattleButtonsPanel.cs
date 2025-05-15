using BattleCruisers.UI.Commands;
using BattleCruisers.Data;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : ButtonsPanel
    {
        public ButtonController nextButton, clockedGameButton;

        public void Initialise(
            IPostBattleScreen postBattleScreen,
            ICommand nextCommand,
            ICommand clockedGameCommand,
            SingleSoundPlayer soundPlayer,
            bool wasVictory)
        {
            Debug.Log($"Initializing PostBattleButtonsPanel with SelectedLevel: {DataProvider.GameModel.SelectedLevel}, Mode: {ApplicationModel.Mode}");

            base.Initialise(postBattleScreen, soundPlayer);

            Helper.AssertIsNotNull(nextCommand, clockedGameButton);
            Assert.IsNotNull(nextCommand);

            CanvasGroupButton retryButton = transform.FindNamedComponent<CanvasGroupButton>("RetryButton");
            retryButton.Initialise(soundPlayer, postBattleScreen.Retry);
            if (wasVictory)
            {
                Destroy(retryButton.gameObject);
            }
            CanvasGroupButton loadoutButton = transform.FindNamedComponent<CanvasGroupButton>("LoadoutButton");
            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);

            if (clockedGameCommand.CanExecute)
            {
                Destroy(homeButton.gameObject);
                Destroy(loadoutButton.gameObject);
            }

            nextButton.Initialise(soundPlayer, nextCommand);
            if (DataProvider.GameModel.SelectedLevel >= 32 || ApplicationModel.Mode == GameMode.SideQuest)
            {
                Destroy(nextButton.gameObject);
            }
            clockedGameButton.Initialise(soundPlayer, clockedGameCommand);
        }
    }
}