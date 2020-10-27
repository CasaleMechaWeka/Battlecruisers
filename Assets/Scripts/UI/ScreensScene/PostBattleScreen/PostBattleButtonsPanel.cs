using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : ButtonsPanel
    {
        public void Initialise(
            IPostBattleScreen postBattleScreen, 
            ICommand nextCommand, 
            ISingleSoundPlayer soundPlayer,
            bool wasVictory)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Assert.IsNotNull(nextCommand);

            CanvasGroupButton retryButton = transform.FindNamedComponent<CanvasGroupButton>("SmallButtons/RetryButton");
            retryButton.Initialise(soundPlayer, postBattleScreen.Retry);
            if (wasVictory)
            {
                Destroy(retryButton.gameObject);
            }

            CanvasGroupButton loadoutButton = transform.FindNamedComponent<CanvasGroupButton>("SmallButtons/LoadoutButton");
            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);

            ButtonController nextButton = GetComponentInChildren<ButtonController>();
            Assert.IsNotNull(nextButton);
            nextButton.Initialise(soundPlayer, nextCommand);
        }
    }
}