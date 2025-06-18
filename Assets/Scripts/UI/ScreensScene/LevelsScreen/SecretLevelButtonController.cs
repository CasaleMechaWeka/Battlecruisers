using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class SecretLevelButtonController : ElementWithClickSound
    {
        private ScreensSceneGod _screensSceneGod;

        [SerializeField]
        private int _levelNum;

        public GameObject captainImage;

        protected override SoundKey ClickSound => SoundKeys.UI.Click;

        public void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer,
            int numOfLevelUnlocked)
        {
            _screensSceneGod = screensSceneGod;

            // Call the base class Initialise method with the required parameters
            base.Initialise(soundPlayer);

            Enabled = numOfLevelUnlocked >= _levelNum;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.GoToTrashScreen(_levelNum);
        }

        protected override void ShowEnabledState()
        {
            captainImage.SetActive(Enabled);
        }

        protected override void ShowDisabledState()
        {
            captainImage.SetActive(false);
        }
    }
}
