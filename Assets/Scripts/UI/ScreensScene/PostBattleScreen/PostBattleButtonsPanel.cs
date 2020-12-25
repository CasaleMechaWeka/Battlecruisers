using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : ButtonsPanel
    {
        public ButtonController nextButton, clockedGameButton;

        public void Initialise(
            IPostBattleScreen postBattleScreen, 
            ICommand nextCommand, 
            ICommand clockedGameCommand,
            ISingleSoundPlayer soundPlayer,
            bool wasVictory)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Helper.AssertIsNotNull(nextCommand, clockedGameButton);
            Assert.IsNotNull(nextCommand);

            CanvasGroupButton retryButton = transform.FindNamedComponent<CanvasGroupButton>("SmallButtons/RetryButton");
            retryButton.Initialise(soundPlayer, postBattleScreen.Retry);
            if (wasVictory)
            {
                Destroy(retryButton.gameObject);
            }
            CanvasGroupButton loadoutButton = transform.FindNamedComponent<CanvasGroupButton>("SmallButtons/LoadoutButton");
            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);

            if (clockedGameCommand.CanExecute)
            {
                Destroy(homeButton.gameObject);
                Destroy(loadoutButton.gameObject);
            }

            nextButton.Initialise(soundPlayer, nextCommand);
            clockedGameButton.Initialise(soundPlayer, clockedGameCommand);
        }
    }
}