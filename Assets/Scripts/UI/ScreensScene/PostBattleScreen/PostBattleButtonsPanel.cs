using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : ButtonsPanel
    {
        public void Initialise(IPostBattleScreen postBattleScreen, ICommand nextCommand, ISoundPlayer soundPlayer)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Assert.IsNotNull(nextCommand);

            ActionButton retryButton = transform.FindNamedComponent<ActionButton>("SmallButtons/RetryButton");
            retryButton.Initialise(soundPlayer, postBattleScreen.Retry);

            ActionButton loadoutButton = transform.FindNamedComponent<ActionButton>("SmallButtons/LoadoutButton");
            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);

            ButtonController nextButton = GetComponentInChildren<ButtonController>();
            Assert.IsNotNull(nextButton);
            nextButton.Initialise(soundPlayer, nextCommand);
        }
    }
}