using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CancelButtonController : CanvasGroupButton
    {
        private InfiniteLoadoutScreenController _loadoutScreen;

        public void Initialise(SingleSoundPlayer soundPlayer, InfiniteLoadoutScreenController loadoutScreen)
        {
            base.Initialise(soundPlayer, parent: loadoutScreen);

            Assert.IsNotNull(loadoutScreen);
            _loadoutScreen = loadoutScreen;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _loadoutScreen.Cancel();
        }
    }
}