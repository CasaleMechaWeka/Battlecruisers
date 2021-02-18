using BattleCruisers.UI.ScreensScene.PostBattleScreen.States;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostSkirmishButtonsPanel : ButtonsPanel
    {
        public CanvasGroupButton loadoutButton, retryButton;

        public void Initialise(
            IPostBattleScreen postBattleScreen,
            ISingleSoundPlayer soundPlayer,
            PostSkirmishState postSkirmishState,
            bool wasVictory)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Helper.AssertIsNotNull(loadoutButton, retryButton);
            Assert.IsNotNull(postSkirmishState);

            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);
            
            retryButton.Initialise(soundPlayer, postSkirmishState.RetrySkirmish);
            if (wasVictory)
            {
                Destroy(retryButton.gameObject);
            }
        }
    }
}