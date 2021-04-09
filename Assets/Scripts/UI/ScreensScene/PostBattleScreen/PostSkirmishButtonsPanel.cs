using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostSkirmishButtonsPanel : ButtonsPanel
    {
        public CanvasGroupButton loadoutButton, retryButton;

        public void Initialise(
            IPostBattleScreen postBattleScreen,
            ISingleSoundPlayer soundPlayer,
            bool wasVictory)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Helper.AssertIsNotNull(loadoutButton, retryButton);

            loadoutButton.Initialise(soundPlayer, postBattleScreen.GoToLoadoutScreen);
            
            retryButton.Initialise(soundPlayer, postBattleScreen.RetrySkirmish);
            if (wasVictory)
            {
                Destroy(retryButton.gameObject);
            }

            gameObject.SetActive(true);
        }
    }
}