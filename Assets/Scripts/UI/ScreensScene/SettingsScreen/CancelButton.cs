using System;
using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : TextButton
    {
        private IScreensSceneGod _screensSceneGod;

        public void Initialise(ISingleSoundPlayer soundPlayer, IScreensSceneGod screensSceneGod, IPresentable parent)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(screensSceneGod, parent);

            _screensSceneGod = screensSceneGod;
            parent.Dismissed += Parent_Dismissed;
        }

        // FELIX  Avoid duplicate code? :/
        private void Parent_Dismissed(object sender, EventArgs e)
        {
            Reset();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.GoToHomeScreen();
        }
    }
}