using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CancelButtonController : CanvasGroupButton
    {
        private ILoadoutScreenController _loadoutScreen;

        public void Initialise(ISingleSoundPlayer soundPlayer, ILoadoutScreenController loadoutScreen)
        {
            base.Initialise(soundPlayer, loadoutScreen);

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