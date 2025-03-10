using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : TextButton
    {
        private ScreenController _settingsScreen;

        public void Initialise(ISingleSoundPlayer soundPlayer, ScreenController settingsScreen)
        {
            base.Initialise(soundPlayer, settingsScreen);

            Assert.IsNotNull(settingsScreen);
            _settingsScreen = settingsScreen;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _settingsScreen.Cancel();
        }
    }
}